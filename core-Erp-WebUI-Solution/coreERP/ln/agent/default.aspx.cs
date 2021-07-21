using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.agent
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
            string[] str = new string[] {"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z" };
            string[] str2 = new string[] { "ABC", "DEF", "GHI", "JKL", "MNO", "PQR", "STU", "VWX", "YZ" };
            var rnode = new RadTreeNode ("Agent","_root_");
            tree.Nodes.Add(rnode);
            rnode.Expanded = true;
            RadTreeNode node=null;
            int j = 0;
            for (int i = 0; i < 26; i++)
            {
                if (i % 3 == 0)
                {
                    node = new RadTreeNode(str2[j], str2[j]);
                    j++;
                    rnode.Nodes.Add(node);
                }
                string str3 = str[i];
                RadTreeNode node2 = new RadTreeNode(str3, str3);
                node.Nodes.Add(node2);
                var agents = le.agents.Where(p=>p.surName.StartsWith(str3) || p.surName.StartsWith(str3.ToLower())).ToList();
                foreach (var cl in agents)
                {
                    RadTreeNode node3 = new RadTreeNode(cl.surName + ", " + cl.otherNames, cl.agentNo);
                    node3.NavigateUrl = "/ln/agent/agent.aspx?id=" + cl.agentID.ToString();
                    node2.Nodes.Add(node3);
                }
            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            List<coreLogic.agent> agents = null;
            if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccNo.Text.Trim().Length > 0)
                agents = le.agents.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.agentNo.Contains(txtAccNo.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccNo.Text.Trim().Length > 0)
                agents = le.agents.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.agentNo.Contains(txtAccNo.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccNo.Text.Trim().Length > 0)
                agents = le.agents.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.agentNo.Contains(txtAccNo.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccNo.Text.Trim().Length == 0)
                agents = le.agents.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccNo.Text.Trim().Length > 0)
                agents = le.agents.Where(p => p.agentNo.Contains(txtAccNo.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccNo.Text.Trim().Length == 0)
                agents = le.agents.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccNo.Text.Trim().Length == 0)
                agents = le.agents.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).ToList();
            else
                agents = le.agents.ToList();
            Session["agents"] = agents;
            grid.DataSource = agents;
            grid.DataBind();
        }

        protected void grid_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            if (Session["agents"] != null)
            {
                grid.DataSource = Session["agents"];
                grid.DataBind();
            }
        }
    }
}