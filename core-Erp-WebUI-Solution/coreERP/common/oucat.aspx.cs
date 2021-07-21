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

namespace coreERP.common
{
    public partial class oucat : corePage
    {
        public override string URL
        {
            get { return "~/common/oucat.aspx"; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                RenderTree();
            }
        }

        private void RenderTree()
        {
            this.RadTreeView1.Nodes.Clear();
            RadTreeNode rootNode = new RadTreeNode("The Company", "__root__");
            rootNode.ImageUrl = "~/images/tree/folder_open.jpg";
            rootNode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
            this.RadTreeView1.Nodes.Add(rootNode);
            core_dbEntities ent = new core_dbEntities();
            var categories = (from cat in ent.ou_cat where cat.parent_ou_cat_id.Equals(null) select cat).ToList<ou_cat>();
            foreach (ou_cat cat in categories)
            {
                RenderTree(rootNode, cat);
            }
            this.RadTreeView1.ExpandAllNodes();
        }

        private void RenderTree(RadTreeNode parentNode, ou_cat category)
        { 
            RadTreeNode node = new RadTreeNode(category.cat_name, category.ou_cat_id.ToString());
            node.Visible = true;
            node.ImageUrl = "~/images/tree/folder_open.jpg";
            node.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
            parentNode.Nodes.Add(node);
            core_dbEntities ent = new core_dbEntities();
            var categories = (from cat in ent.ou_cat where cat.parent_ou_cat_id==category.ou_cat_id select cat).ToList<ou_cat>();
            foreach (ou_cat cat in categories)
            {
                RenderTree(node, cat);
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
            RadTreeNode clickedNode = e.Node;
            core_dbEntities ent = new core_dbEntities();

            switch (e.MenuItem.Value)
            { 
                case "New":
                    RadTreeNode newCat = new RadTreeNode("New OU Category");
                    newCat.Selected = true;
                    newCat.ImageUrl = clickedNode.ImageUrl;
                    newCat.ExpandedImageUrl = clickedNode.ExpandedImageUrl;
                    clickedNode.Nodes.Add(newCat);
                    clickedNode.Expanded = true;
                    
                    clickedNode.Font.Bold = true;
                    //set node's value so we can find it in startNodeInEditMode
                    newCat.Value = "-1";
                    startNodeInEditMode(newCat.Value);
                    break; 
                case "Delete":
                    int catID = int.Parse(e.Node.Value);
                    var cats = (from cat in ent.ou_cat where cat.ou_cat_id==catID select cat).ToList<ou_cat>();
                    if (cats.Count == 1)
                    {
                        ent.ou_cat.Remove(cats[0]);
                        ent.SaveChanges();
                    }
                    clickedNode.Remove();
                    RenderTree();
                    break;
            }
        }

        private void startNodeInEditMode(string nodeValue)
        {
            //find the node by its Value and edit it when page loads
            string js = "Sys.Application.add_load(editNode); function editNode(){ ";
            js += "var tree = $find(\"" + RadTreeView1.ClientID + "\");";
            js += "var node = tree.findNodeByValue('" + nodeValue + "');";
            js += "if (node) node.startEdit();";
            js += "Sys.Application.remove_load(editNode);};";

            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "nodeEdit", js, true);
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

    }
}
