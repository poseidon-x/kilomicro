using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.staff
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

        private void RenderTree(RadTreeNode node2)
        {
            var str3 = node2.Value;
            var staffs = le.staffs.Where(p => p.surName.StartsWith(str3) || p.surName.StartsWith(str3.ToLower())).ToList();
            foreach (var cl in staffs)
            {
                RadTreeNode node3 = new RadTreeNode(cl.surName + ", " + cl.otherNames, cl.staffNo);
                node3.NavigateUrl = "/fa/staff/staff.aspx?id=" + cl.staffID.ToString();
                node3.ImageUrl = "~/images/new.jpg";
                node3.ExpandedImageUrl = "~/images/new.jpg";
                node2.Nodes.Add(node3);
            }
        }

        private void RenderTree()
        {
            string[] str = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            string[] str2 = new string[] { "ABC", "DEF", "GHI", "JKL", "MNO", "PQR", "STU", "VWX", "YZ" };
            var rnode = new RadTreeNode("Staff", "_root_");
            rnode.ImageUrl = "~/images/tree/folder_open.jpg";
            rnode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
            tree.Nodes.Add(rnode);
            rnode.Expanded = true;
            RadTreeNode node = null;
            int j = 0;
            for (int i = 0; i < 26; i++)
            {
                if (i % 3 == 0)
                {
                    node = new RadTreeNode(str2[j], str2[j]);
                    j++;
                    node.ImageUrl = "~/images/tree/folder_open.jpg";
                    node.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                    rnode.Nodes.Add(node);
                }
                string str3 = str[i];
                RadTreeNode node2 = new RadTreeNode(str3, str3);
                node2.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                node2.ImageUrl = "~/images/tree/folder_open.jpg";
                node2.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                node.Nodes.Add(node2);
            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            List<coreLogic.staff> staffs = null;
            if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccNo.Text.Trim().Length > 0)
                staffs = le.staffs.Where(p => p.surName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.staffNo.Contains(txtAccNo.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccNo.Text.Trim().Length > 0)
                staffs = le.staffs.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.staffNo.Contains(txtAccNo.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccNo.Text.Trim().Length > 0)
                staffs = le.staffs.Where(p => p.surName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.staffNo.Contains(txtAccNo.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccNo.Text.Trim().Length == 0)
                staffs = le.staffs.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.surName.Contains(txtSurname.Text.Trim()) ).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccNo.Text.Trim().Length > 0)
                staffs = le.staffs.Where(p => p.staffNo.Contains(txtAccNo.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccNo.Text.Trim().Length == 0)
                staffs = le.staffs.Where(p => p.surName.Contains(txtSurname.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccNo.Text.Trim().Length == 0)
                staffs = le.staffs.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).ToList();
            else
                staffs = le.staffs.ToList();
            grid.DataSource = staffs;
            grid.DataBind();
        }

        protected void tree_NodeExpand(object sender, RadTreeNodeEventArgs e)
        {
            RenderTree(e.Node);
        }
    }
}