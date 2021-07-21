using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.client
{
    public partial class _default : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        string categoryID = null;
        private int serialNumber = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            categoryID = Request.Params["catID"];
            le = new coreLogic.coreLoansEntities();
            if(!IsPostBack)
            {
                RenderTree();
            }
        }

        private void RenderTree()
        {
            var rnode = new RadTreeNode("Clients", "_root_");
            tree.Nodes.Add(rnode);
            rnode.Expanded = true;
            rnode.ImageUrl = "~/images/tree/folder_open.jpg";
            rnode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
            RadTreeNode node = null;

            var branches = (
                    from c in le.clients
                    from b in le.branches
                    where c.branchID == b.branchID
                    select new
                    {
                        b.branchID,
                        b.branchName
                    }
                ).Distinct().OrderBy(p => p.branchName); 
            foreach (var r in branches)
            { 
                node = new RadTreeNode(r.branchName, "b:" + r.branchID.ToString()); 
                node.ImageUrl = "~/images/tree/folder_open.jpg";
                node.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                node.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                rnode.Nodes.Add(node);  
            }
        }

        private void RenderTreeBranch(RadTreeNode rnode)
        {
            var bID = int.Parse(rnode.Value.Split(':')[1]);
            string[] str = new string[] {"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z" };
            string[] str2 = new string[] { "ABC", "DEF", "GHI", "JKL", "MNO", "PQR", "STU", "VWX", "YZ" }; 
            RadTreeNode node=null;

            int j = -1;
            var init = false;
            for (int i = 0; i < 26; i++)
            {
                if (i % 3 == 0)
                {
                    init = false;
                    j++;
                }
                string str3 = str[i];
                var count = le.clients.Where(p => (p.branchID == bID) &&
                    (((p.clientTypeID == 4 || p.clientTypeID == 5) ? p.companyName : ((p.clientTypeID == 6) ? p.accountName : p.surName)).StartsWith(str3) ||
                    ((p.clientTypeID == 4 || p.clientTypeID == 5) ? p.companyName : ((p.clientTypeID == 6) ? p.accountName : p.surName)).StartsWith(str3.ToLower()))).Count();
                if (count > 0)
                {
                    if (j < 9 && init == false)
                    {
                        init = true;
                        node = new RadTreeNode(str2[j], str2[j]);
                        node.ImageUrl = "~/images/tree/folder_open.jpg";
                        node.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                        rnode.Nodes.Add(node);
                    }
                    RadTreeNode node2 = new RadTreeNode(str3, "a:" + str3 + ":" + bID.ToString());
                    node2.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                    node2.ImageUrl = "~/images/tree/folder_open.jpg";
                    node2.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                    node.Nodes.Add(node2);
                }
            }
        }

        private void RenderTreeAlpha(RadTreeNode node2)
        {
            var str = node2.Value.Split(':')[1];
            var bID = int.Parse(node2.Value.Split(':')[2]);
            var clients = (from p in le.clients
                           where (
                                (((p.clientTypeID == 4 || p.clientTypeID == 5) ? p.companyName : ((p.clientTypeID == 6) ? p.accountName : p.surName)).StartsWith(str) ||
                                    ((p.clientTypeID == 4 || p.clientTypeID == 5) ? p.companyName : ((p.clientTypeID == 6) ? p.accountName : p.surName)).StartsWith(str.ToLower())))
                                && (p.branchID == bID)
                           select new
                           {
                               subName = ((p.clientTypeID == 4 || p.clientTypeID == 5) ? p.companyName : ((p.clientTypeID == 6) ? p.accountName : p.surName)).Substring(0,2)
                           }).Distinct().OrderBy(p=> p.subName).ToList();
            foreach (var cl in clients)
            {
                RadTreeNode node3 = new RadTreeNode(cl.subName.ToUpper(), "u:" + cl.subName + ":" + bID.ToString());
                node3.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                node3.ImageUrl = "~/images/tree/folder_open.jpg";
                node3.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                node2.Nodes.Add(node3);
            }
        }

        private void RenderTree(RadTreeNode node2)
        {
            var str = node2.Value.Split(':')[1];
            var bID = int.Parse(node2.Value.Split(':')[2]);
            var clients = le.clients.Where(p => (
                (((p.clientTypeID == 4 || p.clientTypeID == 5) ? p.companyName : ((p.clientTypeID==6)?p.accountName:p.surName)).StartsWith(str) ||
                    ((p.clientTypeID == 4 || p.clientTypeID == 5) ? p.companyName : ((p.clientTypeID == 6) ? p.accountName : p.surName)).StartsWith(str.ToLower())))
                && (p.branchID == bID)).OrderBy(p => p.accountNumber).ToList();
            foreach (var cl in clients)
            {
                RadTreeNode node3 = new RadTreeNode(((cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ?cl.accountName : cl.surName + ", " + cl.otherNames)) + " (" + cl.accountNumber + ")", cl.accountNumber);
                node3.NavigateUrl = "/ln/client/client.aspx?id=" + cl.clientID.ToString() + "&catID=" + cl.categoryID.ToString();
                ////cl.clientImages.Load();
                var img = cl.clientImages.FirstOrDefault();
                if (img == null)
                {
                    node3.ImageUrl = "~/images/new.jpg";
                }
                else
                {
                    node3.ImageUrl = "~/ln/loans/image.aspx?id=" + img.imageID.ToString() + "&h=24&w=24";
                }
                node3.ExpandedImageUrl = "~/images/new.jpg";
                node2.Nodes.Add(node3);
            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            var surName = txtSurname.Text.Trim().Replace(";", "");
            var otherNames = txtOthernames.Text.Trim().Replace(";", "");
            int? catID = -1;
            if (categoryID != null) catID = int.Parse(categoryID);
            List<coreLogic.client> clients = null;
            if (surName.Length > 0 && otherNames.Length > 0 && txtAccNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.surName.ToLower().Contains(surName.ToLower()) || p.accountName.ToLower().Contains(surName.ToLower())).Where(
                    p => p.otherNames.ToLower().Contains(otherNames.ToLower())).Where(
                    p => p.accountNumber.Contains(txtAccNo.Text.Trim())).OrderBy(p => p.accountNumber).ToList();
            else if (surName.Length == 0 && otherNames.Length > 0 && txtAccNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.otherNames.ToLower().Contains(otherNames.ToLower())).Where(
                    p => p.accountNumber.Contains(txtAccNo.Text.Trim())).OrderBy(p => p.accountNumber).ToList();
            else if (surName.Length > 0 && otherNames.Length == 0 && txtAccNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.surName.ToLower().Contains(surName.ToLower()) || p.accountName.ToLower().Contains(surName.ToLower())).Where(
                    p => p.accountNumber.Contains(txtAccNo.Text.Trim())).OrderBy(p => p.accountNumber).ToList();
            else if (surName.Length > 0 && otherNames.Length > 0 && txtAccNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.ToLower().Contains(otherNames.ToLower())).Where(
                    p => p.surName.ToLower().Contains(surName.ToLower()) || p.accountName.ToLower().Contains(surName.ToLower())).OrderBy(p => p.accountNumber).ToList();
            else if (surName.Length == 0 && otherNames.Length == 0 && txtAccNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.accountNumber.Contains(txtAccNo.Text.Trim())).OrderBy(p => p.accountNumber).ToList();
            else if (surName.Length > 0 && otherNames.Length == 0 && txtAccNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.surName.ToLower().Contains(surName.ToLower()) || p.accountName.ToLower().Contains(surName.ToLower())).OrderBy(p => p.accountNumber).ToList();
            else if (surName.Length == 0 && otherNames.Length > 0 && txtAccNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.ToLower().Contains(otherNames.ToLower())).OrderBy(p => p.accountNumber).ToList();
            else
                clients = le.clients.OrderBy(p => p.accountNumber).ToList();
            Session["clients"] = clients;
            for (var i = clients.Count - 1; i >= 0; i--)
            {
                var cl = clients[i];
                //cl.branchReference.Load();
                if (cl.clientTypeID == 6)
                {
                    cl.surName = cl.accountName;
                    cl.otherNames = "";
                }
                else if (cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5)
                {
                    cl.surName = cl.companyName;
                    cl.otherNames = "";
                }
            }
            grid.DataSource = clients;
            grid.DataBind();
        }

        protected void grid_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            if (Session["clients"] != null)
            {
                grid.DataSource = Session["clients"];
                grid.DataBind();
            }
        }

        protected void tree_NodeExpand(object sender, RadTreeNodeEventArgs e)
        {
            if (e.Node.Value.StartsWith("b"))
            {
                RenderTreeBranch(e.Node);
            }
            else if (e.Node.Value.StartsWith("a"))
            {
                RenderTreeAlpha(e.Node);
            }
            else if (e.Node.Value.StartsWith("u"))
            {
                RenderTree(e.Node);
            }
        }

        [WebMethod]
        public static AutoCompleteBoxData GetClientSurNames(object context)
        {
            var le = new coreLogic.coreLoansEntities();
            string searchString = ((Dictionary<string, object>)context)["Text"].ToString().ToLower();
            var data = (from c in le.clients
                        where c.surName.ToLower().Contains(searchString)
                        select new
                        {
                            c.surName
                        }).OrderBy(p => p.surName).Distinct().ToList();
            List<AutoCompleteBoxItemData> result = new List<AutoCompleteBoxItemData>();

            foreach (var row in data)
            {
                AutoCompleteBoxItemData childNode = new AutoCompleteBoxItemData();
                childNode.Text = row.surName;
                childNode.Value = row.surName;
                result.Add(childNode);
            }

            AutoCompleteBoxData res = new AutoCompleteBoxData();
            res.Items = result.ToArray();

            return res;
        }

        [WebMethod]
        public static AutoCompleteBoxData GetClientOtherNames(object context)
        {
            var le = new coreLogic.coreLoansEntities();
            string searchString = ((Dictionary<string, object>)context)["Text"].ToString().ToLower();
            var data = (from c in le.clients
                        where c.otherNames.ToLower().Contains(searchString)
                        select new
                        {
                            c.otherNames
                        }).OrderBy(p => p.otherNames).Distinct().ToList();
            List<AutoCompleteBoxItemData> result = new List<AutoCompleteBoxItemData>();

            foreach (var row in data)
            {
                AutoCompleteBoxItemData childNode = new AutoCompleteBoxItemData();
                childNode.Text = row.otherNames;
                childNode.Value = row.otherNames;
                result.Add(childNode);
            }

            AutoCompleteBoxData res = new AutoCompleteBoxData();
            res.Items = result.ToArray();

            return res;
        }
 
        public string GetSerialNumber()
        {
            serialNumber = serialNumber + 1;

            return serialNumber.ToString("#,##0");
        }
    }
}