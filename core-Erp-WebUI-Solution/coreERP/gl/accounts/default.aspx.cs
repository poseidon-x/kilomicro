using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreLogic;
using System.Data;
using Telerik.Web.UI;

namespace coreERP.gl.accounts
{
    public partial class _default : corePage
    {
        public override string URL
        {
            get { return "~/gl/accounts/default.aspx"; }
        }



        core_dbEntities ent;
        bool doubleClickFlag = false;
        protected void Page_Load(object sender, EventArgs e)
        { 
            ent = new core_dbEntities();
            divError.Style["visibility"] = "hidden"; 
            if (!Page.IsPostBack)
            {
                RenderTree();
                cboCur_ItemsRequested();
            }
            divProc.Style["visibility"] = "hidden";
        }

        void RadTreeView1_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RenderTree()
        {
            try
            {
                this.RadTreeView1.Nodes.Clear();
                RadTreeNode rootNode = new RadTreeNode("Chart of Accounts", "__root__");
                rootNode.ImageUrl = "~/images/tree/folder_open.jpg";
                rootNode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                rootNode.ToolTip = "Right Click to setup the main account heads";
                rootNode.Expanded = true;
                this.RadTreeView1.Nodes.Add(rootNode);
                var categories = (from cat in ent.acct_cats  
                                  orderby cat.cat_code ascending
                                  select cat).ToList<acct_cats>();
                foreach (acct_cats category in categories)
                {
                    RadTreeNode node = new RadTreeNode(category.cat_name, "c:" + category.acct_cat_id.ToString()+":0");
                    node.Visible = true;
                    node.ImageUrl = "~/images/chart_of_accounts/accountHead.jpg";
                    node.ExpandedImageUrl = "~/images/chart_of_accounts/accountHead.jpg";
                    node.ToolTip = category.cat_name + " | Accounts: " +
                        category.min_acct_num + " -> " + category.max_acct_num;
                    rootNode.Nodes.Add(node);
                    var heads = (from head in ent.acct_heads
                                 where head.acct_cats.acct_cat_id == category.acct_cat_id &&
                                    head.parent_acct_head_id == null
                                 orderby head.min_acct_num
                                 select head).ToList<acct_heads>();
                    foreach (var head in heads)
                    {
                        RenderTree(node, category, head);
                    }
                }
                //this.RadTreeView1.ExpandAllNodes();
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        private void RenderTree(RadTreeNode parentNode, acct_cats category, acct_heads head)
        {
            RadTreeNode node = new RadTreeNode(head.head_name, "h:" + head.acct_head_id.ToString()+":0");
            node.Visible = true;
            node.ImageUrl = "~/images/chart_of_accounts/accountSubHead.jpg";
            node.ExpandedImageUrl = "~/images/chart_of_accounts/accountSubHead.jpg";
            node.ToolTip = head.head_name + " | Accounts: " +
                head.min_acct_num + " -> " + head.max_acct_num;
            parentNode.Nodes.Add(node);
            RenderTree(node, head);
            var heads = (from h in ent.acct_heads
                         where h.parent_acct_head_id == head.acct_head_id
                         orderby h.min_acct_num
                         select h).ToList<acct_heads>();
            foreach (var h in heads)
            {
                RenderTree(node, category, h);
            }
        }

        private void RenderTree(RadTreeNode parentNode, acct_heads head)
        { 
            var accs = (from h in ent.accts
                         where h.acct_heads.acct_head_id == head.acct_head_id
                         orderby h.acc_num
                         select h).ToList<accts>();
            foreach (var h in accs)
            {
                RadTreeNode node = new RadTreeNode(h.acc_name + " (" + h.acc_num + ")", "a:" + h.acct_id.ToString() + ":0");
                node.ImageUrl = "~/images/chart_of_accounts/account.jpg";
                node.ExpandedImageUrl = "~/images/chart_of_accounts/account.jpg";
                //node.ToolTip = head.head_name + " | Accounts: " +
                //    head.min_acct_num + " -> " + head.max_acct_num;
                parentNode.Nodes.Add(node);
            }
        }

        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            //RadGrid1.ExportSettings.ExportOnlyData = true;
            //RadGrid1.ExportSettings.IgnorePaging = true;
            //RadGrid1.ExportSettings.OpenInNewWindow = true;
            //this.RadGrid1.ExportSettings.FileName = "coreERP_ou_categories";
            //RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, System.EventArgs e)
        {
            //RadGrid1.ExportSettings.ExportOnlyData = true;
            //RadGrid1.ExportSettings.IgnorePaging = true;
            //RadGrid1.ExportSettings.OpenInNewWindow = true;
            //this.RadGrid1.ExportSettings.FileName = "coreERP_ou_categories";
            //RadGrid1.MasterTableView.ExportToWord();
        }

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            //RadGrid1.ExportSettings.ExportOnlyData = true;
            //RadGrid1.ExportSettings.IgnorePaging = true;
            //RadGrid1.ExportSettings.OpenInNewWindow = true;
            //this.RadGrid1.ExportSettings.FileName = "coreERP_ou_categories";
            //this.RadGrid1.ExportSettings.Pdf.Title = "Currencies Defined in System";
            //this.RadGrid1.ExportSettings.Pdf.AllowModify = false;
            //RadGrid1.MasterTableView.ExportToPdf();
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
                        if (itemType == "__root__")
                        {
                            RadTreeNode newCat = new RadTreeNode("New Account Head");
                            newCat.Selected = true;
                            newCat.ImageUrl = "~/images/chart_of_accounts/accountHead.jpg";
                            newCat.ExpandedImageUrl = "~/images/chart_of_accounts/accountHead.jpg";
                            clickedNode.Nodes.Add(newCat);
                            clickedNode.Expanded = true;

                            clickedNode.Font.Bold = true;
                            //set node's value so we can find it in startNodeInEditMode
                            newCat.Value = "-1";
                            Clear();
                            this.pnlEditCat.Visible = true;
                        }
                        else if (itemType == "c")
                        {
                            RadTreeNode newCat = new RadTreeNode("New Account Sub Head");
                            newCat.Selected = true;
                            newCat.ImageUrl = "~/images/chart_of_accounts/accountSubHead.jpg";
                            newCat.ExpandedImageUrl = "~/images/chart_of_accounts/accountSubHead.jpg";
                            clickedNode.Nodes.Add(newCat);
                            clickedNode.Expanded = true;

                            clickedNode.Font.Bold = true;
                            //set node's value so we can find it in startNodeInEditMode
                            newCat.Value = "-1";
                            ClearHead();
                            txtCatID.Text = clickedNode.Value.Split(':')[1];
                            
                            var n = clickedNode;
                            var p = n.Text; int check = 0;
                            n = n.ParentNode;
                            while (n.Value != "__root__")
                            {
                                p = n.Text + " -> " + p;
                                n = n.ParentNode; 
                                check++;
                                if (check > 100) break;
                            }
                            lblPath.Text = p;
                            this.pnlEditHead.Visible = true;
                        }
                        else if (itemType == "h")
                        {
                            RadTreeNode newCat = new RadTreeNode("New Account Sub Head");
                            newCat.Selected = true;
                            newCat.ImageUrl = "~/images/chart_of_accounts/accountSubHead.jpg";
                            newCat.ExpandedImageUrl = "~/images/chart_of_accounts/accountSubHead.jpg";
                            clickedNode.Nodes.Add(newCat);
                            clickedNode.Expanded = true;

                            clickedNode.Font.Bold = true;
                            //set node's value so we can find it in startNodeInEditMode
                            newCat.Value = "-1";
                            ClearHead();
                            txtParentHeadID.Text = clickedNode.Value.Split(':')[1];
                            var n = clickedNode;
                            var p = n.Text; int check = 0;
                            n = n.ParentNode;
                            while (n.Value != "__root__")
                            {
                                p = n.Text + " -> " + p;
                                n = n.ParentNode;
                                check++;
                                if (check > 100) break;
                            }
                            lblPath.Text = p;
                            this.pnlEditHead.Visible = true;
                        }
                        break;
                    case "NewAccount":
                        itemType = clickedNode.Value.Split(':')[0];
                        if (itemType == "h")
                        {
                            RadTreeNode newCat = new RadTreeNode("New Account");
                            newCat.Selected = true;
                            newCat.ImageUrl = "~/images/chart_of_accounts/account.jpg";
                            newCat.ExpandedImageUrl = "~/images/chart_of_accounts/account.jpg";
                            clickedNode.Nodes.Add(newCat);
                            clickedNode.Expanded = true;

                            clickedNode.Font.Bold = true;
                            //set node's value so we can find it in startNodeInEditMode
                            newCat.Value = "-1";
                            ClearAcc();  
                            txtParentHeadID.Text = clickedNode.Value.Split(':')[1];
                            cboCur_ItemsRequested(); 
                            cboCur.SelectedValue = ent.currencies.FirstOrDefault<currencies>().currency_id.ToString();
                            var n = clickedNode;
                            var p = n.Text; int check = 0;
                            n = n.ParentNode;
                            while (n.Value != "__root__")
                            {
                                p = n.Text + " -> " + p;
                                n = n.ParentNode;
                                check++;
                                if (check > 100) break;
                            }
                            this.lblAccountHead.Text = p;
                            this.pnlEditAccount.Visible = true;
                            //this.cboCur.DataBind();
                        }
                        break;
                    case "Delete":
                        itemType = clickedNode.Value.Split(':')[0];
                        if (itemType == "c")
                        {
                            int catID = int.Parse(e.Node.Value.Split(':')[1]);
                            var cats = (from org in ent.acct_cats where org.acct_cat_id == catID select org).ToList<coreLogic.acct_cats>();
                            if (cats.Count == 1)
                            {
                                ent.acct_cats.Remove(cats[0]);
                                ent.SaveChanges();
                            }
                            clickedNode.ExpandParentNodes();
                            clickedNode.Remove();
                            RenderTree();
                        }
                        else if (itemType == "h")
                        {
                            int headID = int.Parse(e.Node.Value.Split(':')[1]);
                            var heads = (from org in ent.acct_heads where org.acct_head_id == headID select org).ToList<coreLogic.acct_heads>();
                            if (heads.Count == 1)
                            {
                                ent.acct_heads.Add(heads[0]);
                                ent.SaveChanges();
                            }
                            clickedNode.ExpandParentNodes();
                            clickedNode.Remove();
                            RenderTree();
                        }
                        else if (itemType == "a")
                        {
                            int editedAccID = int.Parse(e.Node.Value.Split(':')[1]);
                            var accs = (from org in ent.accts where org.acct_id == editedAccID select org).ToList<coreLogic.accts>();
                            if (accs.Count == 1)
                            {
                                var acc = accs[0];
                                ent.accts.Remove(acc);
                                ent.SaveChanges();
                            }
                            clickedNode.ExpandParentNodes();
                            clickedNode.Remove();
                            RenderTree();
                        }
                        break;
                    case "Edit":
                        itemType = clickedNode.Value.Split(':')[0];
                        if (itemType == "c")
                        {
                            int editedCatID = int.Parse(e.Node.Value.Split(':')[1]);
                            var cats = (from org in ent.acct_cats where org.acct_cat_id == editedCatID select org).ToList<coreLogic.acct_cats>();
                            if (cats.Count == 1)
                            {
                                var cat = cats[0];

                                txtCode.Text=cat.cat_code.ToString();
                                txtCatName.Text=cat.cat_name;
                                txtMax.Text=cat.max_acct_num;
                                txtMin.Text=cat.min_acct_num;
                                txtCatID.Text = cat.acct_cat_id.ToString();
                                pnlEditCat.Visible = true;
                            }
                        }
                        else if (itemType == "h")
                        {
                            int editedHeadID = int.Parse(e.Node.Value.Split(':')[1]);
                            var heads = (from org in ent.acct_heads where org.acct_head_id == editedHeadID select org).ToList<coreLogic.acct_heads>();
                            if (heads.Count == 1)
                            {
                                var head = heads[0];

                                txtHeadID.Text = head.acct_head_id.ToString();
                                txtHeadMax.Text = head.max_acct_num;
                                txtHeadMin.Text = head.min_acct_num;
                                txtHeadName.Text = head.head_name;
                                txtParentHeadID.Text = (head.parent_acct_head_id == null) ? "" : head.parent_acct_head_id.ToString();
                                pnlEditHead.Visible = true;
                                var n = clickedNode;
                                n = n.ParentNode;
                                var p = n.Text; int check = 0;
                                n = n.ParentNode;
                                while (n.Value != "__root__")
                                {
                                    p = n.Text + " -> " + p;
                                    n = n.ParentNode;
                                    check++;
                                    if (check > 100) break;
                                }
                                lblPath.Text = p;
                            }
                        }
                        else if (itemType == "a")
                        {
                            int editedAccID = int.Parse(e.Node.Value.Split(':')[1]);
                            var accs = (from account in ent.accts 
                                        where account.acct_id == editedAccID 
                                        select new {
                                            account,
                                            account.currencies.currency_id,
                                            account.acct_heads.acct_head_id
                                        });
                            foreach (var acc in accs)
                            { 

                                txtAccName.Text = acc.account.acc_name;
                                txtAccNum.Text = acc.account.acc_num;
                                cboCur.SelectedValue = acc.currency_id.ToString();
                                ckAccActive.Checked = acc.account.acc_is_active;
                                txtAccID.Text = acc.account.acct_id.ToString();
                                txtParentHeadID.Text = acc.acct_head_id.ToString();
                                pnlEditAccount.Visible = true;
                                cboCur_ItemsRequested();
                                cboCur.SelectedValue = acc.currency_id.ToString();
                                var n = clickedNode;
                                n = n.ParentNode;
                                var p = n.Text; int check = 0;
                                n = n.ParentNode;
                                while (n.Value != "__root__")
                                {
                                    p = n.Text + " -> " + p;
                                    n = n.ParentNode;
                                    check++;
                                    if (check > 100) break;
                                }
                                this.lblAccountHead.Text = p;
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

        protected void RadTreeView1_NodeEdit(object sender, RadTreeNodeEditEventArgs e)
        {
            core_dbEntities ent = new core_dbEntities();
            if (e.Node.Value == "-1")
            {
                ou_cat cat = new ou_cat();
                cat.cat_name = e.Text;
                cat.creation_date = DateTime.Now;
                cat.creator = User.Identity.Name;
                if (e.Node.ParentNode.Value != "__root__")
                {
                    cat.parent_ou_cat_id = int.Parse(e.Node.ParentNode.Value);
                }
                ent.ou_cat.Add(cat);
                ent.SaveChanges();
                e.Node.Value = cat.ou_cat_id.ToString();
            }
            else
            {
                int catID = int.Parse(e.Node.Value);
                var cats = (from cat in ent.ou_cat where cat.ou_cat_id == catID select cat).ToList<ou_cat>();
                if (cats.Count == 1)
                {
                    var cat = cats[0];
                    cat.cat_name = e.Text;
                    cat.modification_date = DateTime.Now;
                    cat.last_modifier = User.Identity.Name;
                    ent.SaveChanges();
                }
            }
            e.Node.Text = e.Text;
        }

        private void Clear()
        {
            this.txtCatName.Text = "";
            this.txtCode.Text = "";
            this.txtMax.Text = "";
            this.txtMin.Text = "";
            txtCatID.Text = "-1";
            this.spanError.InnerHtml = "";
        }

        private void ClearHead()
        {
            txtHeadName.Text = "";
            txtHeadMin.Text = "";
            txtHeadMax.Text = "";
            txtHeadID.Text = "-1";
            txtParentHeadID.Text = "";
            this.spanError.InnerHtml = "";
        }

        private void ClearAcc()
        {
            txtAccID.Text = "-1";
            txtAccName.Text = "";
            txtAccNum.Text = "";
            this.spanError.InnerHtml = "";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        { 
                int newID = -1;
                if (txtCatID.Text == "-1")
                {
                    int catID = int.Parse(txtCatID.Text);
                    coreLogic.acct_cats cat= new coreLogic.acct_cats();
                    cat.cat_code = byte.Parse(txtCode.Text);
                    cat.cat_name = txtCatName.Text;
                    cat.max_acct_num = txtMax.Text;
                    cat.min_acct_num = txtMin.Text;
                    cat.creation_date = DateTime.Now;
                    cat.creator = User.Identity.Name;  
                    ent.acct_cats.Add(cat);
                    ent.SaveChanges();
                    newID = cat.acct_cat_id;
                    Clear();
                    this.pnlEditCat.Visible = false; 
                }
                else
                {
                    int catID = int.Parse(txtCatID.Text);
                    var cats = (from org in ent.acct_cats where org.acct_cat_id == catID select org).ToList<coreLogic.acct_cats>();
                    if (cats.Count == 1)
                    {
                        var cat = cats[0];
                        cat.cat_code = byte.Parse(txtCode.Text);
                        cat.cat_name = txtCatName.Text;
                        cat.max_acct_num = txtMax.Text;
                        cat.min_acct_num = txtMin.Text;
                        cat.modification_date = DateTime.Now;
                        cat.last_modifier = User.Identity.Name;
                        ent.SaveChanges();
                        newID = cat.acct_cat_id;
                        Clear();
                        this.pnlEditCat.Visible = false;
                    }
                }
                if (newID != -1)
                {
                    this.RenderTree();
                    RadTreeView1.CollapseAllNodes();
                    var editedNode = RadTreeView1.FindNodeByValue("c:" + newID.ToString() + ":0");
                    if (editedNode != null)
                    {
                        editedNode.ExpandParentNodes();
                    }
                } 
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
            pnlEditCat.Visible = false;
            RenderTree();
        }

        protected void btnSaveHead_Click(object sender, EventArgs e)
        {
            int newID = -1;
            if (txtHeadID.Text == "-1")
            {
                if (txtParentHeadID.Text != "")
                {
                    int parentHeadID = int.Parse(txtParentHeadID.Text);
                    var ph = (from c in ent.acct_heads where c.acct_head_id == parentHeadID select c).ToList<acct_heads>()[0];
                    var cat = (from c in ent.acct_cats
                               from h in ent.acct_heads
                               where c.acct_cat_id == h.acct_cats.acct_cat_id
                                && h.acct_head_id == parentHeadID
                               select c).ToList<acct_cats>()[0];
                    coreLogic.acct_heads head = new coreLogic.acct_heads();
                    head.head_name = txtHeadName.Text;
                    head.max_acct_num = txtHeadMax.Text;
                    head.min_acct_num = txtHeadMin.Text;
                    head.acct_cats = cat;
                    head.parent_acct_head_id = ph.acct_head_id;
                    head.creation_date = DateTime.Now;
                    head.creator = User.Identity.Name;
                    ent.acct_heads.Add(head);
                    ent.SaveChanges();
                    newID = head.acct_head_id;
                    ClearHead();
                    this.pnlEditHead.Visible = false;
                }
                else
                {
                    int catID = int.Parse(txtCatID.Text);
                    var cat = (from c in ent.acct_cats where c.acct_cat_id == catID select c).ToList<acct_cats>()[0];
                    coreLogic.acct_heads head = new coreLogic.acct_heads();
                    head.head_name = txtHeadName.Text;
                    head.max_acct_num = txtHeadMax.Text;
                    head.min_acct_num = txtHeadMin.Text;
                    head.acct_cats = cat;
                    if (txtParentHeadID.Text != "") head.parent_acct_head_id = int.Parse(txtParentHeadID.Text);
                    head.creation_date = DateTime.Now;
                    head.creator = User.Identity.Name;
                    ent.acct_heads.Add(head);
                    ent.SaveChanges();
                    newID = head.acct_head_id;
                    ClearHead();
                    this.pnlEditHead.Visible = false;
                }
            }
            else
            {
                int headID = int.Parse(txtHeadID.Text);
                var heads = (from org in ent.acct_heads where org.acct_head_id == headID select org).ToList<coreLogic.acct_heads>();
                if (heads.Count == 1)
                {
                    var head = heads[0];
                    head.head_name = txtHeadName.Text;
                    head.max_acct_num = txtHeadMax.Text;
                    head.min_acct_num = txtHeadMin.Text;
                    head.modification_date = DateTime.Now;
                    head.last_modifier = User.Identity.Name;
                    ent.SaveChanges();
                    newID = newID = head.acct_head_id;
                    ClearHead();
                    this.pnlEditHead.Visible = false;
                }

            }
            if (newID != -1)
            {
                this.RenderTree();
                RadTreeView1.CollapseAllNodes();
                var editedNode = RadTreeView1.FindNodeByValue("h:" + newID.ToString() + ":0");
                if (editedNode != null)
                {
                    editedNode.ExpandParentNodes();
                }
            }
        }

        protected void btnCancelHead_Click(object sender, EventArgs e)
        {
            ClearHead();
            pnlEditHead.Visible = false;
            RenderTree();
        }

        protected void btnSaveAcc_Click(object sender, EventArgs e)
        { 
                int newID = -1;
                int currencyID = int.Parse(cboCur.SelectedValue);
                if (txtAccID.Text == "-1")
                {
                    if (txtParentHeadID.Text != "")
                    {
                        int parentHeadID = int.Parse(txtParentHeadID.Text);
                        var ph = (from c in ent.acct_heads where c.acct_head_id == parentHeadID select c).ToList<acct_heads>()[0];
                        coreLogic.accts acc = new coreLogic.accts();
                        acc.acc_is_active = ckAccActive.Checked;
                        acc.acc_name = txtAccName.Text;
                        acc.acc_num = txtAccNum.Text;
                        acc.acct_heads = ph;
                        acc.acc_is_active = ckAccActive.Checked;
                        acc.currencies = (from c in ent.currencies where c.currency_id == currencyID select c).ToList<currencies>()[0];
                        acc.creation_date = DateTime.Now;
                        acc.creator = User.Identity.Name;
                        ent.accts.Add(acc);
                        ent.SaveChanges();
                        newID = acc.acct_id;
                        ClearHead();
                        this.pnlEditAccount.Visible = false;
                    } 
                }
                else
                {
                    int accID = int.Parse(txtAccID.Text);
                    var accs = (from org in ent.accts where org.acct_id == accID select org).ToList<coreLogic.accts>();
                    if (accs.Count == 1)
                    {
                        var acc = accs[0];
                        acc.acc_is_active = ckAccActive.Checked;
                        acc.acc_name = txtAccName.Text;
                        acc.acc_num = txtAccNum.Text;
                        acc.acc_is_active = ckAccActive.Checked;
                        acc.currencies = (from c in ent.currencies where c.currency_id == currencyID select c).ToList<currencies>()[0];
                        acc.modification_date = DateTime.Now;
                        acc.last_modifier = User.Identity.Name;
                        ent.SaveChanges();
                        newID = newID = acc.acct_id;
                        ClearHead();
                        this.pnlEditAccount.Visible = false;
                    }

                }
                if (newID != -1)
                {
                    this.RenderTree();
                    RadTreeView1.CollapseAllNodes();
                    var editedNode = RadTreeView1.FindNodeByValue("a:" + newID.ToString() + ":0");
                    if (editedNode != null)
                    {
                        editedNode.ExpandParentNodes();
                    }
                } 
        }

        protected void btnCancelAcc_Click(object sender, EventArgs e)
        {
            ClearAcc();
            this.pnlEditAccount.Visible = false;
            RenderTree();
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

        protected void cboCur_ItemsRequested()
        {
            cboCur.Items.Clear();
            foreach (var cur in ent.currencies.ToList<currencies>())
            {
                RadComboBoxItem item = new RadComboBoxItem();

                item.Text = cur.major_name + " (" + cur.major_symbol +")";
                item.Value = cur.currency_id.ToString();
                 
                item.Attributes.Add("current_buy_rate", cur.current_buy_rate.ToString());
                item.Attributes.Add("major_symbol", cur.major_symbol.ToString());

                cboCur.Items.Add(item);

                item.DataBind();
            }
          }

        protected void RadTreeView1_HandleDrop(object sender, RadTreeNodeDragDropEventArgs e)
        {
            var src = e.SourceDragNode;
            var dest = e.DestDragNode;

            if (src != null && dest != null)
            {
                var nodeType1 = dest.Value.Split(':')[0];
                var nodeType2 = src.Value.Split(':')[0];
                if (nodeType1 == "__root__" || (nodeType1 == "a")
                     || nodeType2 == "__root__" || (nodeType1 == "c" && nodeType2 == "a")
                     || nodeType2 == "c")
                {
                    return;
                }
                else
                {
                    var targetID = dest.Value.Split(':')[1];
                    var sourceID = src.Value.Split(':')[1];
                    if (nodeType2 == "h") {
                        if (nodeType1 == "c")
                        {
                            var headID = int.Parse(sourceID);
                            var catID = int.Parse(targetID);
                            var head = ent.acct_heads.First<acct_heads>(p => p.acct_head_id == headID);
                            var cat = ent.acct_cats.First<acct_cats>(p => p.acct_cat_id == catID);
                            head.acct_cats = cat;
                            head.parent_acct_head_id = null;
                            Recurse(headID,cat);
                            ent.SaveChanges();
                            RenderTree();
                        }
                        else if (nodeType1 == "h")
                        {
                            var headID = int.Parse(sourceID);
                            var pHeadID = int.Parse(targetID);
                            var head = ent.acct_heads.First<acct_heads>(p => p.acct_head_id == headID);
                            var pHead = ent.acct_heads.First<acct_heads>(p => p.acct_head_id == pHeadID);
                            var cat=(from h in ent.acct_heads 
                                                  from c in ent.acct_cats
                                              where h.acct_cats.acct_cat_id==c.acct_cat_id
                                                  && h.acct_head_id == pHeadID
                                              select c).First<acct_cats>();
                            head.acct_cats = cat;//ent.acct_cats.First<acct_cats>(p => p.acct_cat_id == pHead.);
                            head.parent_acct_head_id = pHeadID; 
                            Recurse(headID,cat);
                            ent.SaveChanges();
                            RenderTree();
                        }
                    }
                    else if (nodeType2 == "a")
                    {
                        var accounID = int.Parse(sourceID);
                        var headID = int.Parse(targetID);
                        var acc = ent.accts.First<accts>(p => p.acct_id == accounID);
                        acc.acct_heads = ent.acct_heads.First<acct_heads>(p => p.acct_head_id == headID);
                        ent.SaveChanges();
                        RenderTree();
                    }
                }
            }
        }

        private void Recurse(int headID, acct_cats cat)
        {
            var heads = (from e in ent.acct_heads
                         where e.parent_acct_head_id == headID
                         select e);
            foreach (var h in heads)
            {
                h.acct_cats = cat;
                Recurse(h.acct_head_id, cat);
            }
        }
    }
}
