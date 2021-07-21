using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.asset
{
    public partial class _default : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;

        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            if(!IsPostBack)
            {
                RenderTree();
            }
        }

        private void RenderTree()
        {
            coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
            string[] str = new string[] {"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z" };
            string[] str2 = new string[] { "ABC", "DEF", "GHI", "JKL", "MNO", "PQR", "STU", "VWX", "YZ" };
            tree.Nodes.Clear();
            var rnode = new RadTreeNode ("Assets","_root_");
            tree.Nodes.Add(rnode);
            rnode.Expanded = true;
            RadTreeNode node=null;
            int j = 0;
            foreach (var r in le.assetCategories)
            {
                node = new RadTreeNode(r.assetCategoryName, "C:" + r.assetCategoryID.ToString());
                rnode.Nodes.Add(node);
                //r.assetSubCategories.Load();
                foreach (var r2 in r.assetSubCategories)
                { 
                    RadTreeNode node2 = new RadTreeNode(r2.assetSubCategoryName, "S:"+r2.assetSubCategoryID.ToString());
                    node.Nodes.Add(node2);
                    var deps = le.assets.Where(p => p.assetSubCategoryID == r2.assetSubCategoryID).Select(p => p.ouID).Distinct();
                    foreach (var dep in deps)
                    {
                        var d = ent.ou.Where(p=>p.ou_id==dep).FirstOrDefault();
                        var dept = "No Department";
                        if (d != null)
                        {
                            dept = d.ou_name;
                        }
                        RadTreeNode node3 = new RadTreeNode(dept, dept + r2.assetSubCategoryID.ToString());
                        node2.Nodes.Add(node3);
                        var assets = le.assets.Where(p => p.assetSubCategoryID == r2.assetSubCategoryID).ToList();
                        foreach (var cl in assets)
                        {
                            if (cl.ouID == dep)
                            {
                                RadTreeNode node4 = new RadTreeNode(cl.assetDescription, cl.assetTag);
                                node4.NavigateUrl = "/fa/assets/asset.aspx?id=" + cl.assetID.ToString();
                                node3.Nodes.Add(node4);
                            }
                        }
                    }
                }
            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            List<coreLogic.asset> assets = null;
            if (txtDesc.Text.Trim().Length > 0 && txtAccNo.Text.Trim().Length > 0)
                assets = le.assets.Where(p => p.assetDescription.Contains(txtDesc.Text.Trim())).Where(
                    p => p.assetTag.Contains(txtAccNo.Text.Trim())).ToList();
            else if (txtDesc.Text.Trim().Length == 0 && txtAccNo.Text.Trim().Length > 0)
                assets = le.assets.Where(
                    p => p.assetTag.Contains(txtAccNo.Text.Trim())).ToList();
            else if (txtDesc.Text.Trim().Length > 0 && txtAccNo.Text.Trim().Length > 0)
                assets = le.assets.Where(p => p.assetDescription.Contains(txtDesc.Text.Trim())).Where(
                    p => p.assetTag.Contains(txtAccNo.Text.Trim())).ToList();
            else if (txtDesc.Text.Trim().Length > 0 && txtAccNo.Text.Trim().Length == 0)
                assets = le.assets.Where(
                    p => p.assetDescription.Contains(txtDesc.Text.Trim())).ToList();
            else if (txtDesc.Text.Trim().Length == 0 && txtAccNo.Text.Trim().Length > 0)
                assets = le.assets.Where(p => p.assetTag.Contains(txtAccNo.Text.Trim())).ToList();
            else if (txtDesc.Text.Trim().Length > 0 && txtAccNo.Text.Trim().Length == 0)
                assets = le.assets.Where(p => p.assetDescription.Contains(txtDesc.Text.Trim())).ToList();
            else if (txtDesc.Text.Trim().Length == 0 && txtAccNo.Text.Trim().Length == 0)
                assets = le.assets.ToList();
            else
                assets = le.assets.ToList();
            grid.DataSource = assets;
            grid.DataBind();
        }
    }
}