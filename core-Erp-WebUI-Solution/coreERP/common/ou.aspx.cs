using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreLogic;
using System.Data.Entity;
using Telerik.Web.UI;
using System.Text.RegularExpressions;
using System.Data;

namespace coreERP.common
{
    public partial class ou : corePage
    {
        public override string URL
        {
            get { return "~/common/ou.aspx"; }
        }


        core_dbEntities ent;
        protected void Page_Load(object sender, EventArgs e)
        {
            ent = new core_dbEntities();
            divError.Style["visibility"] = "hidden";
            if (!Page.IsPostBack)
            {
                RenderTree();
            }
            divProc.Style["visibility"] = "hidden";
        }

        private void RenderTree()
        {
            try
            {
                this.RadTreeView1.Nodes.Clear();
                RadTreeNode rootNode = new RadTreeNode("The Company", "__root__");
                rootNode.ImageUrl = "~/images/tree/folder_open.jpg";
                rootNode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                this.RadTreeView1.Nodes.Add(rootNode);
                var categories = (from cat in ent.ou_cat where cat.parent_ou_cat_id.Equals(null) select cat).ToList<ou_cat>();
                foreach (ou_cat category in categories)
                {
                    RadTreeNode node = new RadTreeNode(toPlural(category.cat_name), "c:" + category.ou_cat_id.ToString());
                    node.Visible = true;
                    node.ImageUrl = "~/images/tree/folder_open.jpg";
                    node.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                    rootNode.Nodes.Add(node);
                    var ous = (from org in ent.ou where org.ou_cat.ou_cat_id == category.ou_cat_id select org).ToList<coreLogic.ou>();
                    foreach (coreLogic.ou org in ous)
                    {
                        RenderTree(node, category, org);
                    }
                }
                this.RadTreeView1.ExpandAllNodes();
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
         
        private void RenderTree(RadTreeNode parentNode, ou_cat category, coreLogic.ou org)
        {
            RadTreeNode node = new RadTreeNode(org.ou_name, "o:" + org.ou_id.ToString());
            node.Visible = true;
            node.ImageUrl = "~/images/ou/dept.jpg";
            node.ExpandedImageUrl = "~/images/ou/dept.jpg";
            parentNode.Nodes.Add(node); 
            var categories = (from cat in ent.ou_cat where cat.parent_ou_cat_id == category.ou_cat_id select cat).ToList<ou_cat>();
            foreach (ou_cat cat in categories)
            {
                RadTreeNode cnode = new RadTreeNode(toPlural(cat.cat_name), "c:" + cat.ou_cat_id.ToString());
                cnode.Visible = true;
                cnode.ImageUrl = "~/images/tree/folder_open.jpg";
                cnode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                node.Nodes.Add(cnode);

                var ous = (from org2 in ent.ou where org2.ou_cat.ou_cat_id == cat.ou_cat_id 
                           && org2.parent_ou_id == org.ou_id select org2).ToList<coreLogic.ou>();
                 foreach (coreLogic.ou org2 in ous)
                {
                    RenderTree(cnode, cat, org2);
                }
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
                        RadTreeNode newCat = new RadTreeNode("New " + clickedNode.Text);
                        newCat.Selected = true;
                        newCat.ImageUrl = "~/images/ou/dept.jpg";
                        newCat.ExpandedImageUrl = "~/images/ou/dept.jpg";
                        clickedNode.Nodes.Add(newCat);
                        clickedNode.Expanded = true;

                        clickedNode.Font.Bold = true;
                        //set node's value so we can find it in startNodeInEditMode
                        newCat.Value = "-1";
                        this.txtCategoryName.Text = clickedNode.Text;
                        this.txtUnitName.Text = "New " + clickedNode.Text;
                        this.txtCatID.Text = clickedNode.Value.Split(':')[1];
                        if (clickedNode.ParentNode.Value != "__root__")
                        {
                            txtParentOrgID.Text = clickedNode.ParentNode.Value.Split(':')[1];
                        }
                        else
                        {
                            txtParentOrgID.Text = "";
                        }
                        this.txtOrgID.Text = "-1";
                        this.pnlEdit.Visible = true;
                        break;
                    case "Delete":
                        int ouID = int.Parse(e.Node.Value.Split(':')[1]);
                        var ous = (from org in ent.ou where org.ou_id == ouID select org).ToList<coreLogic.ou>();
                        if (ous.Count == 1)
                        {
                            ent.ou.Remove(ous[0]);
                            ent.SaveChanges();
                        }
                        clickedNode.Remove();
                        RenderTree();
                        break;
                    case "Edit":
                        int editedOUID = int.Parse(e.Node.Value.Split(':')[1]);
                        var ous2 = (from org in ent.ou where org.ou_id == editedOUID select org).ToList<coreLogic.ou>();
                        if (ous2.Count == 1)
                        {
                            var org = ous2[0];
                            this.txtCategoryName.Text = clickedNode.ParentNode.Text;
                            this.txtUnitName.Text = org.ou_name;
                            this.txtCatID.Text = clickedNode.ParentNode.Value.Split(':')[1];
                            this.txtParentOrgID.Text = org.parent_ou_id.ToString();
                            this.txtOrgID.Text = org.ou_id.ToString();
                            this.pnlEdit.Visible = true;
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
            txtCatID.Text = "";
            txtCategoryName.Text = "";
            txtUnitName.Text = "";
            txtOrgID.Text = "";
            cboUnitManager.SelectedIndex = -1;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtOrgID.Text == "-1")
                {
                    int catID = int.Parse(txtCatID.Text);
                    var cats = (from cat in ent.ou_cat where cat.ou_cat_id == catID select cat).ToList<ou_cat>();
                    coreLogic.ou org = new coreLogic.ou();
                    org.ou_name = txtUnitName.Text;
                    org.creation_date = DateTime.Now;
                    org.creator = User.Identity.Name;
                    org.ou_cat = cats[0];
                    if (txtParentOrgID.Text != "") org.parent_ou_id = int.Parse(txtParentOrgID.Text);
                    ent.ou.Add(org);
                    ent.SaveChanges();
                    Clear();
                    pnlEdit.Visible = false;
                    this.RenderTree();
                }
                else
                {
                    int orgID = int.Parse(txtOrgID.Text);
                    var ous = (from org in ent.ou where org.ou_id == orgID select org).ToList<coreLogic.ou>();
                    if (ous.Count == 1)
                    {
                        var org = ous[0];
                        org.ou_name = txtUnitName.Text;
                        org.modification_date = DateTime.Now;
                        org.last_modifier = User.Identity.Name;
                        ent.SaveChanges();
                        Clear();
                        pnlEdit.Visible = false;
                        this.RenderTree();
                    }
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        { 
            Clear();
            pnlEdit.Visible = false;
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
                if (ex.InnerException.Message.Contains("uk_ou_name"))
                {
                    errorMsg += "\nThe Organizational Unit you are trying to create already exist.";
                }
            }
            errorMsg += "Please correct and continue or cancel.";
            divError.Style["visibility"] = "visible";
            spanError.InnerHtml = errorMsg;
        }
    }
}
