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
    public partial class v : corePage
    {
        public override string URL
        {
            get { return "~/gl/pc/pc.aspx"; }
        }

        private void AddNew()
        {
            v_type = Session["v_type"].ToString();
            batch = new v_head
            {
                batch_no="",
                creation_date=DateTime.Now,
                creator=Context.User.Identity.Name,
                currency_id=0,
                bank_acct_id = 0,
                v_type=v_type,
                posted=false,
                rate=1,
                recipient="",
                vat_rate=0,
                with_amt=0,
                with_rate=0,
                vat_amt=0,
                nhil_amt=0,
                nhil_rate=0,
                is_nhil=false,
                is_vat=false,
                is_withheld=false,
                check_no="",
                invoice_no=""
            };
            var profile = ent.comp_prof.FirstOrDefault();
            if (profile != null && profile.currency_id != null)
                Session["currency_id"] = profile.currency_id;
            Session["batch"] = batch;
            Session["tx_count"] = 0;
            dtTransactionDate.SelectedDate = DateTime.Now;
            txtBatchNumber.Text = "UNASSIGNED";
            txtCreator.Text = Context.User.Identity.Name;
            txtBatchState.Text = "UNPOSTED";
            //txtRecipient.Text = batch.recipient;
            SetBalance();
            SetRate();
            DisplayAmounts();
            cboAcc.Enabled = true;
            txtRate.Enabled = true;
            cboRec.Enabled = true;
            cboCur.Enabled = true;
            dtTransactionDate.Enabled = true;
            txtWithRate.Enabled = true;
            txtVATRate.Enabled = true;
            txtWithRate.Enabled = true;
            chkWith.Enabled = true;
            chkVAT.Enabled = true;
            chkNHIL.Enabled = true;
            txtInvoiceNo.Enabled = true;
            txtCheckNo.Enabled = true;
            dtTransactionDate.Enabled = true;
            cboAccW.Enabled = true;
            txtNHILRate.Enabled = true;
            try
            {
                this.EntityDataSource1.WhereParameters["v_head_id"].DefaultValue = batch.v_head_id.ToString();
            }
            catch (Exception ex)
            {
                this.EntityDataSource1.WhereParameters["v_head_id"].DefaultValue = "0";
            }
            try
            {
                this.EntityDataSource3.WhereParameters["v_head_id"].DefaultValue = batch.v_head_id.ToString();
            }
            catch (Exception ex)
            {
                this.EntityDataSource3.WhereParameters["v_head_id"].DefaultValue = "0";
            }
            if (v_type == "C")
            {
                PopulateCustomers();
            }
            else if (v_type == "S")
            {
                PopulateSuppliers();
            }
            this.RadGrid1.Visible = true;
            this.RadGrid2.Visible = false; 
        }

        private void Edit(string batchNo)
        {
            try
            {
                batch = gent.v_head.First(p => p.batch_no == batchNo);
                v_type=batch.v_type;
                if (batch.posted == true) EditPosted(batchNo);
                else
                {
                    Session["batch"] = batch;
                    Session["tx_count"] = gent.v_dtl.Count(p => p.v_head.v_head_id == batch.v_head_id);
                    dtTransactionDate.SelectedDate = gent.v_dtl.First(p => p.v_head.v_head_id == batch.v_head_id).tx_date;
                    txtBatchNumber.Text = batch.batch_no;
                    txtCreator.Text = batch.creator;
                    txtBatchState.Text = "UNPOSTED";
                    //txtRecipient.Text = batch.recipient;
                    Session["currency_id"] = batch.currency_id;
                    Session["bank_acct_id"] = batch.bank_acct_id;
                    Session["with_acct_id"] = batch.with_acct_id;
                    Session["v_type"] = batch.v_type;
                    if (batch.v_type == "C")
                        Session["rec_id"] = batch.cust_id;
                    else if (batch.v_type == "S")
                        Session["rec_id"] = batch.sup_id;
                    cboAcc.SelectedValue = batch.bank_acct_id.ToString();
                    cboCur.SelectedValue = batch.currency_id.ToString();
                    txtRate.Value = batch.rate;
                    txtInvoiceNo.Text = batch.invoice_no;
                    txtCheckNo.Text = batch.check_no;
                    chkNHIL.Checked = batch.is_nhil;
                    chkVAT.Checked = batch.is_vat;
                    chkWith.Checked = batch.is_withheld;
                    txtVATRate.Value = batch.vat_rate;
                    if (batch.v_type == "C" && batch.cust_id != null)
                    {
                        cboRec.SelectedValue = batch.cust_id.ToString();
                    }
                    else if (batch.v_type == "S" && batch.sup_id != null)
                    {
                        cboRec.SelectedValue = batch.sup_id.ToString();
                    }
                    txtNHILRate.Value = batch.nhil_rate;
                    txtWithRate.Value = batch.with_rate;
                    SetBalance();
                    SetRate();
                    DisplayAmounts();
                    SetCaptions(v_type);
                    cboAcc.Enabled = true;
                    txtRate.Enabled = true;
                    cboRec.Enabled = true;
                    cboCur.Enabled = true;
                    txtWithRate.Enabled = true;
                    txtVATRate.Enabled = true;
                    txtWithRate.Enabled = true;
                    chkWith.Enabled = true;
                    chkVAT.Enabled = true;
                    chkNHIL.Enabled = true;
                    txtInvoiceNo.Enabled = true;
                    txtCheckNo.Enabled = true;
                    dtTransactionDate.Enabled = true;
                    cboAccW.Enabled = true;
                    txtNHILRate.Enabled = true;
                    try
                    {
                        this.EntityDataSource1.WhereParameters["v_head_id"].DefaultValue = batch.v_head_id.ToString();
                        this.EntityDataSource3.WhereParameters["v_head_id"].DefaultValue = batch.v_head_id.ToString();
                    }
                    catch (Exception ex)
                    {
                        this.EntityDataSource1.WhereParameters["v_head_id"].DefaultValue = "0";
                    }
                    this.RadGrid1.Visible = true;
                    this.RadGrid2.Visible = false;
                    PopulateAccountsW();
                }
            }
            catch (InvalidOperationException ix)
            {
                EditPosted(batchNo);
            }
        }

        private void EditPosted(string batchNo)
        {
            batch = gent.v_head.First(p => p.batch_no == batchNo);
                v_type=batch.v_type;
            Session["batch"] = batch;
            dtTransactionDate.SelectedDate = batch.creation_date;
            txtBatchNumber.Text = batch.batch_no;
            txtCreator.Text = batch.creator;
            txtBatchState.Text = "POSTED"; 
            //txtRecipient.Text = batch.recipient;
            Session["currency_id"] = batch.currency_id;
            Session["with_acct_id"] = batch.with_acct_id;

            Session["v_type"] = batch.v_type;
            txtInvoiceNo.Text = batch.invoice_no;
            txtCheckNo.Text = batch.check_no;
            chkNHIL.Checked = batch.is_nhil;
            chkVAT.Checked = batch.is_vat;
            chkWith.Checked = batch.is_withheld;
            txtVATRate.Value = batch.vat_rate;
            if (batch.v_type == "C" && batch.cust_id != null)
            {
                cboRec.SelectedValue = batch.cust_id.ToString();
            }
            else if (batch.v_type == "S" && batch.sup_id != null)
            {
                cboRec.SelectedValue = batch.sup_id.ToString();
            }
            txtNHILRate.Value = batch.nhil_rate;
            txtWithRate.Value = batch.with_rate;
            SetBalance();
            SetRate();
            DisplayAmounts();
            SetCaptions(v_type);
            cboAcc.Enabled = false;
            txtRate.Enabled = false;
            cboRec.Enabled = false;
            cboCur.Enabled = false;
            txtWithRate.Enabled = false;
            txtVATRate.Enabled = false;
            txtWithRate.Enabled = false;
            chkWith.Enabled = false;
            chkVAT.Enabled = false;
            chkNHIL.Enabled = false;
            txtInvoiceNo.Enabled = false;
            txtCheckNo.Enabled = false;
            dtTransactionDate.Enabled = false;
            cboAccW.Enabled = false;
            txtNHILRate.Enabled = false;
            try
            {
                this.EntityDataSource2.WhereParameters["v_head_id"].DefaultValue = batch.v_head_id.ToString();
                this.EntityDataSource3.WhereParameters["v_head_id"].DefaultValue = batch.v_head_id.ToString();
            }
            catch (Exception ex)
            {
                this.EntityDataSource2.WhereParameters["v_head_id"].DefaultValue = "0";
            }
            this.RadGrid1.Visible = false;
            this.RadGrid2.Visible = true; 
        }

        core_dbEntities ent;
        coreGLEntities gent;
        v_head batch;
        string v_type;
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
                    if (da > 0)
                        Session["bank_acct_id"] = da;
                    da = (from p in ent.def_accts
                     where p.code == "WA"
                     select p.accts.acct_id).FirstOrDefault();
                    if (da > 0)
                        Session["with_acct_id"] = da;
                    PopulateCurrencies();
                    PopulateAccounts();
                    PopulateAccountsW();
                    SetCaptions(Request.QueryString["type"]); 
                    Session["v_type"] = v_type;
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
                    batch = Session["batch"] as v_head;
                }
                try
                {
                    this.EntityDataSource1.WhereParameters["v_head_id"].DefaultValue = batch.v_head_id.ToString();
                    this.EntityDataSource3.WhereParameters["v_head_id"].DefaultValue = batch.v_head_id.ToString();
                    //this.EntityDataSource2.WhereParameters["batch_id"].DefaultValue = batchPost.jnl_batch_id.ToString();
                }
                catch (Exception ex)
                {
                    this.EntityDataSource1.WhereParameters["v_head_id"].DefaultValue = "0";
                    this.EntityDataSource3.WhereParameters["v_head_id"].DefaultValue = "0";
                }
                divProc.Style["visibility"] = "hidden";
            }
            catch (Exception x){
                ManageException(x);
            }
        }

        private void SetCaptions(string type)
        {
            if (type == null || type == "" ||
               type.ToUpper().Trim() == "C")
           {
                v_type = "C";
                titleH5.InnerText = "Receipt Voucher Transactions (Customers)";
                lblTitle.InnerText = "Edit Receipt Voucher Transactions (Customers)";
                lblRec.InnerText = "Customer";
            }
            else  
            {
                 v_type =type;
                 titleH5.InnerText = "Payment Voucher Transactions (Suppliers)";
                 lblTitle.InnerText = "Edit Payment Voucher Transactions (Suppliers)";
                 lblRec.InnerText = "Supplier";
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
                        var b = gent.v_head.FirstOrDefault(p => p.batch_no == batchNo);
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
                    RadTreeNode rootNode = new RadTreeNode("Voucher", "__root__");
                    rootNode.ImageUrl = "~/images/tree/folder_open.jpg";
                    rootNode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                    this.RadTreeView1.Nodes.Add(rootNode);
                    var years = (from jnl in gent.v_dtl
                                 from batch in gent.v_head
                                 where jnl.v_head.v_head_id == batch.v_head_id
                                 orderby jnl.tx_date.Year descending
                                 select jnl.tx_date.Year).Distinct();
                    foreach (var year in years)
                    {
                        RadTreeNode node = new RadTreeNode(year.ToString(), "y:" + year.ToString());
                        node.Visible = true;
                        node.ImageUrl = "~/images/tree/folder_open.jpg";
                        node.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                        node.ToolTip = "Expand to see PC transactions for year: " + year.ToString();
                        rootNode.Nodes.Add(node);
                        var months = (from jnl in gent.v_dtl
                                      from batch in gent.v_head
                                      where jnl.v_head.v_head_id == batch.v_head_id
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
                            childnode.ToolTip = "Expand to see PC transactions for month: " + year.ToString() +
                                ", " + monthName;
                            node.Nodes.Add(childnode);
                            var days = (from jnl in gent.v_dtl
                                        from batch in gent.v_head
                                        where jnl.v_head.v_head_id == batch.v_head_id
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
                                gchildnode.ToolTip = "Expand to see PC transactions for date: " +
                                    (new DateTime(year, month, day)).ToString("dd-MMM-yyyy");
                                childnode.Nodes.Add(gchildnode);
                                var batches = (from jnl in gent.v_dtl
                                               from batch in gent.v_head
                                               where jnl.v_head.v_head_id == batch.v_head_id
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
                                        gchildnode2.ImageUrl = "~/images/editNew.jpg";
                                        gchildnode2.ExpandedImageUrl = "~/images/editNew.jpg";
                                       
                                        gchildnode2.ToolTip = "Click to see PC transactions for batch no.: " + b.batch_no;
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

        public string AccountName(int v_dtl_id)
        {
            var j = gent.v_dtl.FirstOrDefault(p => p.v_dtl_id == v_dtl_id);
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

        public string CostCenterName(object id)
        {
            if (id == null) return "";
            var v_dtl_id = int.Parse(id.ToString());
            var j = gent.v_dtl.FirstOrDefault(p => p.v_dtl_id == v_dtl_id);
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

        public string AccountNameW(int v_ftr_id)
        {
            var ftr = (from c in gent.v_ftr
                       where c.v_ftr_id == v_ftr_id
                       select c.acct_id).FirstOrDefault();

            var item = (from a in ent.accts
                        where a.acct_id==ftr
                        select new
                        {
                            a.acc_num,
                            a.acc_name
                        }).FirstOrDefault();
            return item == null ? "" : item.acc_num + " - " + item.acc_name;
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
            //try
            //{
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
                    var jnl = new v_dtl();
                    jnl.description = newVals["description"].ToString();
                    jnl.amount = double.Parse(newVals["amount"].ToString());
                    if (newVals["ref_no"] != null && newVals["ref_no"].ToString() != "")
                        jnl.ref_no = newVals["ref_no"].ToString();
                    else if (txtCheckNo.Text.Trim() != "")
                    {
                        jnl.ref_no = txtCheckNo.Text;
                    } 
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
                    //jnl.v_head = batch.v_head_id > 0 ? gent.v_head.First(p => p.v_head_id == batch.v_head_id) : batch;
                    batch.v_dtl.Add(jnl);
                    if (batch.batch_no == null || batch.batch_no == "")
                    {
                        this.txtBatchNumber.Text = coreExtensions.NextGLBatchNumber();
                        gent.v_head.Add(batch);
                    }
                    batch.is_nhil = chkNHIL.Checked;
                    batch.is_vat = chkVAT.Checked;
                    batch.is_withheld = chkWith.Checked;
                    batch.vat_rate = txtVATRate.Value.Value;
                    batch.nhil_rate = txtNHILRate.Value.Value;
                    batch.with_rate = txtWithRate.Value.Value;
                    batch.invoice_no = txtInvoiceNo.Text;
                    batch.check_no = txtCheckNo.Text;
                    if (Session["with_acct_id"] != null && Session["with_acct_id"].ToString()!="0") 
                        batch.with_acct_id = int.Parse((Session["with_acct_id"]).ToString());
                    batch.with_amt = txtWithAmount.Value.Value;
                    batch.vat_amt = txtVATAmount.Value.Value;
                    batch.nhil_amt = txtNHILAmount.Value.Value;
                    batch.batch_no = this.txtBatchNumber.Text;
                    batch.creation_date = DateTime.Now;
                    batch.creator = User.Identity.Name; 
                    batch.bank_acct_id = int.Parse(cboAcc.SelectedValue);
                    batch.currency_id = int.Parse(cboCur.SelectedValue);
                    batch.check_no = txtCheckNo.Text;

                    gent.SaveChanges();
                    //gent.Refresh(System.Data.Entity.Core.Objects.RefreshMode.StoreWins, batch);
                    Session["tx_count"] = (int)Session["tx_count"] + 1;
                    batch = gent.v_head.First(p => p.v_head_id == batch.v_head_id);
                    Session["batch"] = batch;
                    CalculateAmounts(); 
                    this.EntityDataSource1.WhereParameters["v_head_id"].DefaultValue = batch.v_head_id.ToString();
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
        //    }
        //    catch (Exception ex) { ManageException(ex); }
        //}
            }
        private void CalculateAmounts()
        {
            try
            {
                if (batch != null && batch.v_head_id > 0)
                {
                    var dtls = gent.v_dtl.Where(p => p.v_head_id == batch.v_head_id);
                    foreach (var d in dtls)
                    {
                        if (d.description.Contains(" NHIL ") || d.description.Contains(" VAT ")) gent.v_dtl.Remove(d);
                        else
                        {
                            d.amount += d.vat_amt + d.nhil_amt;
                            d.vat_amt = 0;
                            d.nhil_amt = 0;
                        }
                    }
                    gent.SaveChanges();
                    var prof = ent.comp_prof.First();
                    batch = gent.v_head.First(p => p.v_head_id == batch.v_head_id);
                    var total = (from h in gent.v_head
                                 from d in gent.v_dtl
                                 where h.v_head_id == d.v_head_id
                                     && h.v_head_id == batch.v_head_id
                                 select d.amount).Sum();
                    var withAmt = 0.0;
                    var rem = total;
                    if (chkWith.Checked == true && txtWithRate.Value.Value == 0)
                    {
                        txtWithRate.Value = prof.withh_rate;
                    }
                    if (chkWith.Checked == true)
                    {
                        withAmt = total * txtWithRate.Value.Value / 100.0;
                        rem -= withAmt;
                    }
                    if (chkVAT.Checked == true && txtVATRate.Value.Value == 0)
                    {
                        txtVATRate.Value = prof.vat_rate;
                    }
                    if (chkNHIL.Checked == true && txtNHILRate.Value.Value == 0)
                    {
                        txtNHILRate.Value = prof.nhil_rate;
                    }
                    var fs = gent.v_ftr.Where(p => p.v_head_id == batch.v_head_id);
                    var retained = 0.0;

                    var description = (from j in gent.v_dtl
                                       where j.v_head_id == batch.v_head_id
                                       select j.description).FirstOrDefault();
                    var tx_date = (from j in gent.v_dtl
                                   where j.v_head_id == batch.v_head_id
                                   select j.tx_date).FirstOrDefault();
                    var ref_no = (from j in gent.v_dtl
                                  where j.v_head_id == batch.v_head_id
                                  select j.ref_no).FirstOrDefault();
                    var cost_center_id = (from j in gent.v_dtl
                                          where j.v_head_id == batch.v_head_id
                                          select j.gl_ou_id).FirstOrDefault();
                    foreach (var f in fs)
                    {
                        if (f.is_perc == true)
                        {
                            f.tot_amount = total * f.amount / 100.0;
                        }
                        else
                        {
                            f.tot_amount = f.amount;
                        }
                        retained += f.tot_amount;
                    }
                    var vatAmt = 0.0;
                    var nhilAmt = 0.0;
                    batch.is_withheld = chkWith.Checked;
                    batch.is_vat = chkVAT.Checked;
                    batch.is_nhil = chkNHIL.Checked;
                    dtls = gent.v_dtl.Where(p => p.v_head_id == batch.v_head_id);
                    vatAmt = rem * (txtVATRate.Value.Value) / 100.0;
                    nhilAmt = rem * txtNHILRate.Value.Value / 100.0;
                    rem = total - retained;
                    var va = (from a in ent.def_accts
                              where (a.code == "VAT")
                              select a.accts.acct_id).FirstOrDefault();
                    var vb = (from a in ent.def_accts
                              where (a.code == "NHIL")
                              select a.accts.acct_id).FirstOrDefault();
                    foreach (var d in dtls)
                    {
                        var withDA = 0.0;
                        if (chkWith.Checked == true)
                        {
                            withDA=d.amount * txtWithRate.Value.Value / 100.0;
                            d.amount -= withDA;
                        }
                        if (chkVAT.Checked == true && chkNHIL.Checked == true)
                        {
                            d.vat_amt = d.amount * (txtVATRate.Value.Value) / 100.0;
                            d.nhil_amt = d.amount * (txtNHILRate.Value.Value) / 100.0;
                            d.amount -= (d.vat_amt + d.nhil_amt);
                        }
                        else if (chkVAT.Checked == true)
                        {
                            d.vat_amt = d.amount * (txtVATRate.Value.Value) / 100.0;
                            d.amount -= d.vat_amt;
                        }
                        else if (chkNHIL.Checked == true)
                        {
                            d.nhil_amt = d.amount * (txtNHILRate.Value.Value) / 100.0;
                            d.amount -= d.nhil_amt;
                        }
                        d.amount += withDA;
                    }
                    if (chkVAT.Checked == true && va > 0)
                    {
                        var dtl = new v_dtl
                        {
                            acct_id = va,
                            amount = vatAmt,
                            creation_date = DateTime.Now,
                            creator = User.Identity.Name,
                            description = txtVATRate.Value.Value.ToString() + " % VAT :" + description,
                            is_imprest = false,
                            tx_date = tx_date,
                            gl_ou_id = cost_center_id,
                            ref_no = ref_no,
                            v_head_id = batch.v_head_id
                        };
                        gent.v_dtl.Add(dtl);
                    }
                    if (chkNHIL.Checked == true && vb >0)
                    {
                        var dtl = new v_dtl
                        {
                            acct_id = vb,
                            amount = nhilAmt,
                            creation_date = DateTime.Now,
                            creator = User.Identity.Name,
                            description = txtNHILRate.Value.Value.ToString() + " % NHIL :" + description,
                            is_imprest = false,
                            tx_date = tx_date,
                            gl_ou_id = cost_center_id,
                            ref_no = ref_no,
                            v_head_id = batch.v_head_id
                        };
                        gent.v_dtl.Add(dtl);
                    }
                    batch.nhil_rate = txtNHILRate.Value.Value;
                    batch.vat_rate = txtVATRate.Value.Value;
                    batch.with_rate = txtWithRate.Value.Value;
                    batch.vat_amt = vatAmt;
                    batch.nhil_amt = nhilAmt;
                    batch.with_amt = withAmt;
                    batch.invoice_no = txtInvoiceNo.Text;
                    batch.check_no = txtCheckNo.Text;
                    if (!String.IsNullOrWhiteSpace(cboAccW.SelectedValue))
                        batch.with_acct_id = int.Parse(cboAccW.SelectedValue);
                    txtWithAmount.Value = withAmt;
                    txtVATAmount.Value = vatAmt;
                    txtNHILAmount.Value = nhilAmt;
                    txtRetained.Value = retained;
                    gent.SaveChanges();
                    this.EntityDataSource1.WhereParameters["v_head_id"].DefaultValue = batch.v_head_id.ToString();
                    RadGrid1.DataBind();
                }
            }
            catch (Exception) { }
        }

        protected void txtNHILRate_TextChanged(object sender, EventArgs e)
        {
            CalculateAmounts();
        }

        protected void txtVATRate_TextChanged(object sender, EventArgs e)
        {
            CalculateAmounts();
        }

        protected void txtWithRate_TextChanged(object sender, EventArgs e)
        {
            CalculateAmounts();
        }

        private void DisplayAmounts()
        {
            try
            {
                txtNHILRate.Value = batch.nhil_rate;
                txtVATRate.Value = batch.vat_rate;
                txtWithRate.Value = batch.with_rate;
                txtWithAmount.Value = batch.with_amt;
                txtVATAmount.Value = batch.vat_amt;
                txtNHILAmount.Value = batch.nhil_amt;
                chkNHIL.Checked = batch.is_nhil;
                chkVAT.Checked = batch.is_vat;
                chkWith.Checked = batch.is_withheld;
                if(batch.with_acct_id > 0)
                    cboAccW.SelectedValue = batch.with_acct_id.ToString();
                var retained = (from f in gent.v_ftr
                                where f.v_head_id==batch.v_head_id
                                    select f.tot_amount).Sum();
                if (retained != null) txtRetained.Value = retained;
                else txtRetained.Value = 0.0;
            }
            catch (Exception) { }
        }

        protected void chkVAT_CheckedChanged(object sender, EventArgs e)
        {
            CalculateAmounts();
        }

        protected void chkNHIL_CheckedChanged(object sender, EventArgs e)
        {
            CalculateAmounts();
        }

        protected void chkWith_CheckedChanged(object sender, EventArgs e)
        {
            CalculateAmounts();
        }

        protected void RadGrid3_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                Hashtable newVals = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                e.Canceled = true;
                var account_id = int.Parse(Session["f_acct_id"].ToString());
                accts acc = ent.accts.First<accts>(p => p.acct_id == account_id); 
                var jnl = new v_ftr();
                jnl.description = newVals["description"].ToString();
                jnl.amount = double.Parse(newVals["amount"].ToString());
                jnl.acct_id = acc.acct_id;
                if (Session["is_perc"] != null)
                {
                    jnl.is_perc = bool.Parse(Session["is_perc"].ToString());
                }
                if (jnl.is_perc == true)
                    jnl.tot_amount = (jnl.amount / 100.0) * 1;
                else
                    jnl.tot_amount = jnl.amount;
                jnl.creation_date = DateTime.Now;
                jnl.creator = User.Identity.Name;
                jnl.v_head = batch.v_head_id > 0 ? gent.v_head.First(p => p.v_head_id == batch.v_head_id) : batch;
                gent.v_ftr.Add(jnl);
                if (batch.batch_no == null || batch.batch_no == "")
                    this.txtBatchNumber.Text = coreExtensions.NextGLBatchNumber();
                batch.batch_no = this.txtBatchNumber.Text;
                batch.is_nhil = chkNHIL.Checked;
                batch.is_vat = chkVAT.Checked;
                batch.is_withheld = chkWith.Checked;
                batch.vat_rate = txtVATRate.Value.Value;
                batch.nhil_rate = txtNHILRate.Value.Value;
                batch.with_rate = txtWithRate.Value.Value;
                if (Session["with_acct_id"] != null && Session["with_acct_id"].ToString() != "0")
                    batch.with_acct_id = int.Parse((Session["with_acct_id"]).ToString());
                batch.with_amt = txtWithAmount.Value.Value;
                batch.vat_amt = txtVATAmount.Value.Value;
                batch.nhil_amt = txtNHILAmount.Value.Value;
                batch.invoice_no = txtInvoiceNo.Text;
                batch.check_no = txtCheckNo.Text;
                batch.creation_date = DateTime.Now;
                batch.creator = User.Identity.Name;
                batch.bank_acct_id = int.Parse(cboAcc.SelectedValue);
                batch.currency_id = int.Parse(cboCur.SelectedValue);

                gent.SaveChanges();
                CalculateAmounts();
                //gent.Refresh(System.Data.Entity.Core.Objects.RefreshMode.StoreWins, batch);
                batch = gent.v_head.First(p => p.v_head_id == batch.v_head_id);
                Session["batch"] = batch;
                this.EntityDataSource1.WhereParameters["v_head_id"].DefaultValue = batch.v_head_id.ToString();
                Session["acct_id"] = 0;
                RadGrid1.EditIndexes.Clear();
                e.Item.OwnerTableView.IsItemInserted = false;
                e.Item.OwnerTableView.Rebind();
                RenderTree();
            }
            catch (Exception ex) { ManageException(ex); }
        }
        
        protected void chkPerc_CheckedChanged(object sender, EventArgs e)
        {
            Session["is_perc"] = (sender as CheckBox).Checked;
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
                    var v_dtl_id = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["v_dtl_id"];
                    var jnl = (from p in gent.v_dtl where p.v_dtl_id == v_dtl_id select p).First<v_dtl>();

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
                    jnl.acct_id = acc.acct_id;
                    jnl.tx_date = (DateTime)newVals["tx_date"];
                    jnl.modification_date = DateTime.Now;
                    jnl.last_modifier = User.Identity.Name;
                    batch = gent.v_head.FirstOrDefault(p => p.v_head_id == batch.v_head_id);
                    Session["batch"] = batch;
                    batch.is_nhil = chkNHIL.Checked;
                    batch.is_vat = chkVAT.Checked;
                    batch.is_withheld = chkWith.Checked;
                    batch.vat_rate = txtVATRate.Value.Value;
                    batch.nhil_rate = txtNHILRate.Value.Value;
                    batch.with_rate = txtWithRate.Value.Value;
                    if (Session["with_acct_id"] != null && Session["with_acct_id"].ToString() != "0")
                        batch.with_acct_id = int.Parse((Session["with_acct_id"]).ToString());
                    batch.with_amt = txtWithAmount.Value.Value;
                    batch.vat_amt = txtVATAmount.Value.Value;
                    batch.nhil_amt = txtNHILAmount.Value.Value;
                    batch.invoice_no = txtInvoiceNo.Text;
                    batch.check_no = txtCheckNo.Text;
                    batch.last_modifier = User.Identity.Name;
                    batch.modification_date = DateTime.Now;
                    batch.currency_id = int.Parse(cboCur.SelectedValue);
                    batch.rate = txtRate.Value.Value;
                    batch.bank_acct_id = int.Parse(cboAcc.SelectedValue);
                    batch.check_no = txtCheckNo.Text;
                    gent.SaveChanges();
                    CalculateAmounts();
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

        protected void RadGrid3_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                Hashtable newVals = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                e.Canceled = true;
                var v_ftr_id = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["v_ftr_id"];
                var jnl = (from p in gent.v_ftr where p.v_ftr_id == v_ftr_id select p).First<v_ftr>();

                var account_id = int.Parse(Session["f_acct_id"].ToString());
                accts acc = ent.accts.First<accts>(p => p.acct_id == account_id);
                jnl.description = newVals["description"].ToString();
                jnl.amount = double.Parse(newVals["amount"].ToString());
                jnl.acct_id = acc.acct_id;
                if (Session["is_perc"] != null)
                {
                    jnl.is_perc = bool.Parse(Session["is_perc"].ToString());
                }
                if (jnl.is_perc == true)
                    jnl.tot_amount = (jnl.amount / 100.0) * 1;
                else
                    jnl.tot_amount = jnl.amount;
                jnl.creation_date = DateTime.Now;
                jnl.creator = User.Identity.Name;
                jnl.v_head = batch.v_head_id > 0 ? gent.v_head.First(p => p.v_head_id == batch.v_head_id) : batch;
                jnl.modification_date = DateTime.Now;
                jnl.last_modifier = User.Identity.Name;
                batch = gent.v_head.FirstOrDefault(p => p.v_head_id == batch.v_head_id);
                Session["batch"] = batch;
                batch.is_nhil = chkNHIL.Checked;
                batch.is_vat = chkVAT.Checked;
                batch.is_withheld = chkWith.Checked;
                batch.vat_rate = txtVATRate.Value.Value;
                batch.nhil_rate = txtNHILRate.Value.Value;
                batch.with_rate = txtWithRate.Value.Value;
                if (Session["with_acct_id"] != null && Session["with_acct_id"].ToString() != "0")
                    batch.with_acct_id = int.Parse((Session["with_acct_id"]).ToString());
                batch.with_amt = txtWithAmount.Value.Value;
                batch.vat_amt = txtVATAmount.Value.Value;
                batch.nhil_amt = txtNHILAmount.Value.Value;
                batch.last_modifier = User.Identity.Name;
                batch.invoice_no = txtInvoiceNo.Text;
                batch.check_no = txtCheckNo.Text;
                batch.modification_date = DateTime.Now;
                batch.currency_id = int.Parse(cboCur.SelectedValue);
                batch.rate = txtRate.Value.Value;
                batch.bank_acct_id = int.Parse(cboAcc.SelectedValue);
                gent.SaveChanges();
                CalculateAmounts();
                RadGrid1.EditIndexes.Clear();
                RadGrid1.MasterTableView.IsItemInserted = false;
                RadGrid1.MasterTableView.Rebind();
                Session["acct_id"] = 0;
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

        protected void RadGrid3_ItemInserted(object source, Telerik.Web.UI.GridInsertedEventArgs e)
        {
        }

        protected void RadGrid1_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == Telerik.Web.UI.RadGrid.InitInsertCommandName)
            {
                e.Canceled = true;
                var newVals = new System.Collections.Specialized.ListDictionary();
                newVals["v_dtl_id"] = 0;
                newVals["v_head_id"] = 0;
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
                var v_dtl_id = int.Parse(((GridEditableItem)e.Item).GetDataKeyValue("v_dtl_id").ToString());
                Session["acct_id"] = (from c in gent.v_dtl
                                        where c.v_dtl_id == v_dtl_id 
                                        select new { c.acct_id }).FirstOrDefault().acct_id;
                var ou = (from c in gent.v_dtl
                          where c.v_dtl_id == v_dtl_id
                                        select new { c.gl_ou_id }).FirstOrDefault();
                if (ou != null) Session["gl_ou_id"] = ou.gl_ou_id;
                DateTime? txDate = (from c in gent.v_dtl
                                            where c.v_dtl_id == v_dtl_id
                                    select c.tx_date).FirstOrDefault();
                Session["tx_date"] = txDate;
            }
        }

        protected void RadGrid3_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == Telerik.Web.UI.RadGrid.InitInsertCommandName)
            {
                e.Canceled = true;
                var newVals = new System.Collections.Specialized.ListDictionary();
                newVals["v_ftr_id"] = 0;
                newVals["v_head_id"] = 0; 
                newVals["acct_id"] = 0;  
                newVals["description"] = string.Empty;
                newVals["amount"] = 0.0;
                newVals["tot_amount"] = 0.0;
                newVals["is_perc"] = false;
                newVals["creator"] = User.Identity.Name;
                newVals["creation_date"] = DateTime.Now;
                e.Item.OwnerTableView.InsertItem(newVals);
            }
            else if (e.CommandName == "Edit")
            {
                Hashtable newVals = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                var v_ftr_id = int.Parse(((GridEditableItem)e.Item).GetDataKeyValue("v_ftr_id").ToString());
                Session["f_acct_id"] = (from c in gent.v_ftr 
                                        where c.v_ftr_id == v_ftr_id
                                        select new { c.acct_id }).FirstOrDefault().acct_id;
                Session["is_perc"] = (from c in gent.v_ftr
                                          where c.v_ftr_id == v_ftr_id
                                          select new { c.is_perc }).FirstOrDefault().is_perc;
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
                    var accs = (from a in ent.accts
                                from c in ent.currencies
                                from b in ent.bank_accts
                                where a.currencies.currency_id == c.currency_id
                                    && b.accts.acct_id==a.acct_id
                                select new
                                {
                                    a.acct_id,
                                    a.acc_num,
                                    a.acc_name,
                                    c.major_name,
                                    c.major_symbol,
                                    bank= b.bank_branches.banks.bank_name ,
                                    b.bank_acct_num
                                }).ToList();
                    cboAcc.DataSource = accs;
                    cboAcc.DataBind();
                    if (Session["bank_acct_id"] == null || Session["bank_acct_id"].ToString().Trim() == "0")
                    {
                        cboAcc.Items.Insert(0, new RadComboBoxItem("Voucher Account", null));
                    }
                    else
                    {
                        cboAcc.SelectedValue = Session["bank_acct_id"].ToString();
                        SetBalance();
                    }
                }
            }
            catch (Exception ex) { }
        }

        protected void PopulateAccountsW()
        {
            try
            {
                using (core_dbEntities ent = new core_dbEntities())
                {
                    int? id = null;
                    if (Session["with_acct_id"] != null || Session["with_acct_id"].ToString().Trim() != "0")
                    {
                        id = int.Parse(Session["with_acct_id"].ToString());
                    }
                    var accs = (from a in ent.vw_accounts
                                from c in ent.currencies 
                                where  (id == null || a.acct_id == id)
                                select new
                                {
                                    a.acct_id,
                                    a.acc_num,
                                    a.acc_name,
                                    c.major_name,
                                    c.major_symbol,
                                    a.fullname
                                }).ToList();
                    cboAccW.DataSource = accs;
                    cboAccW.DataBind();
                    if (Session["with_acct_id"] == null || Session["with_acct_id"].ToString().Trim() == "0")
                    {
                        cboAccW.Items.Insert(0, new RadComboBoxItem("Withholding Account", null));
                    }
                    else
                    {
                        cboAccW.SelectedValue = Session["with_acct_id"].ToString(); 
                    }
                }
            }
            catch (Exception ex) { }
        }

        protected void PopulateCustomers()
        {
            try
            {
                using (core_dbEntities ent = new core_dbEntities())
                {
                    var accs = (from a in ent.custs 
                                select new
                                {
                                    id=a.cust_id,
                                    comp_name=a.cust_name,
                                    a.acc_num
                                }).ToList();
                    cboRec.DataSource = accs;
                    cboRec.DataBind();
                    if (Session["rec_id"] == null || Session["rec_id"].ToString().Trim() == "0")
                    {
                        cboRec.Items.Insert(0, new RadComboBoxItem("Transaction Customer", null));
                    }
                    else
                    {
                        cboRec.SelectedValue = Session["rec_id"].ToString();
                    }
                }
            }
            catch (Exception ex) { }
        }

        protected void PopulateSuppliers()
        {
            try
            {
                using (core_dbEntities ent = new core_dbEntities())
                {
                    var accs = (from a in ent.sups
                                select new
                                {
                                    id = a.sup_id,
                                    comp_name = a.sup_name,
                                    a.acc_num
                                }).ToList();
                    cboRec.DataSource = accs;
                    cboRec.DataBind();
                    if (Session["rec_id"] == null || Session["rec_id"].ToString().Trim() == "0")
                    {
                        cboRec.Items.Insert(0, new RadComboBoxItem("Transaction Supplier", null));
                    }
                    else
                    {
                        cboRec.SelectedValue = Session["rec_id"].ToString();
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
                    PopulateAccounts(rcb,true);
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

        public void RadGrid3_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode == true)
            {
                var item = e.Item as GridEditableItem;
                var rcb = item["acct_id"].Controls[1] as RadComboBox;
                if (rcb != null)
                {
                    PopulateAccounts(rcb, false);
                    rcb.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cboAccI2_SelectedIndexChanged);
                    rcb.DataBind();
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
                    PopulateAccounts(rcb, true);
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

        void cboAccI2_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            Session["f_acct_id"] = e.Value;
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
                    batch = Session["batch"] as v_head;
                    if (batch != null)
                    {
                        using (var gent = new coreGLEntities())
                        {
                            var b = gent.v_head.FirstOrDefault(p => p.batch_no == batch.batch_no);
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

        protected void cboRec_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                Session["rec_id"] = e.Value; 
                if (Session["batch"] != null)
                {
                    batch = Session["batch"] as v_head;
                    if (batch != null)
                    {
                        using (var gent = new coreGLEntities())
                        {
                            var b = gent.v_head.FirstOrDefault(p => p.batch_no == batch.batch_no);
                            if (b != null)
                            {
                                if (b.v_type == "C")
                                {
                                    b.cust_id = int.Parse(e.Value);
                                }
                                else if (b.v_type == "S")
                                {
                                    b.sup_id = int.Parse(e.Value);
                                }
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
            Session["bank_acct_id"] = e.Value;
            SetBalance();
            CalculateAmounts();
        }

        protected void cboAccW_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            Session["with_acct_id"] = e.Value;
            CalculateAmounts();
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
            if (Session["bank_acct_id"] != null && int.TryParse(Session["bank_acct_id"].ToString(), out accID) == true)
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
                                var b = ent.v_head.FirstOrDefault(p => p.batch_no == batchNo);
                                if (b != null && b.posted == false)
                                {
                                    ent.v_head.Remove(b);
                                    ent.SaveChanges();
                                }
                                else
                                {
                                    ManageException(new ApplicationException("Cannot delete posted Voucher"));
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
                                var b = ent.v_head.FirstOrDefault(p => p.batch_no == batchNo);
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

        public void RadGrid3_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem && batch != null && batch.posted==true)
            {
                Button addButton = e.Item.FindControl("AddNewRecordButton") as Button;
                addButton.Visible = false;
                LinkButton lnkButton = (LinkButton)e.Item.FindControl("InitInsertButton");
                lnkButton.Visible = false;
            }
            if (e.Item is GridEditableItem && batch != null && batch.posted == true)
            {
                var item = e.Item as GridEditableItem;
                var cmd = item["EditCommandColumn"] as TableCell;
                if (cmd != null)
                {
                    cmd.Visible = false;
                }
                var cmd2 = item["DeleteColumn"] as TableCell;
                if (cmd2 != null)
                {
                    cmd2.Visible = false;
                }
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
         
        protected void PopulateAccounts(Telerik.Web.UI.RadComboBox cboAcc, bool isDtl)
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
                            }).ToList();
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
                if (isDtl == true)
                {
                    if (Session["acct_id"] != null && Session["acct_id"].ToString() != "0")
                        cboAcc.SelectedValue = Session["acct_id"].ToString();
                    else
                        cboAcc.Text = " ";
                }
                else
                {
                    if (Session["f_acct_id"] != null && Session["f_acct_id"].ToString() != "0")
                        cboAcc.SelectedValue = Session["f_acct_id"].ToString();
                    else
                        cboAcc.Text = " ";
                }
            }
            catch (Exception ex) { }
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                CalculateAmounts();
                Response.Redirect("~/gl/reports/v/v.aspx?batchno=" + batch.batch_no);
            }
            catch (Exception) { }
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
