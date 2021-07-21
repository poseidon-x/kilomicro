using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using System.Web.Services;
using System.Data;

namespace coreERP.ln.susu
{
    public partial class regularSusuDefault : System.Web.UI.Page
    {

        coreLogic.coreLoansEntities le;
        string categoryID = null;
        private int serialNumber = 0;

        public string GetSerialNumber()
        {
            serialNumber = serialNumber + 1;

            return serialNumber.ToString("#,##0");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            categoryID = Request.Params["catID"];
            if (categoryID == null)
            {
                categoryID = "";
            }
            le = new coreLogic.coreLoansEntities();
            if(!IsPostBack)
            {
                RenderTree();
            }
        }

        private void RenderTree(RadTreeNode node2)
        {
            var startDate = DateTime.ParseExact(node2.Value.Split(':')[1], "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            var bID = int.Parse(node2.Value.Split(':')[2]);
            var endDate = startDate.AddDays(1).AddSeconds(-1);
            var lns = (from l in le.regularSusuAccounts
                       where l.applicationDate >= startDate
                             && l.applicationDate <= endDate
                             && (l.client.branchID == bID)
                       select l).ToList();
            foreach (var ln in lns)
            {
                var amount = 0.0;
                if (ln.regularSusuContributions.Count > 0)
                {
                    amount = ln.regularSusuContributions.Sum(p => p.amount);
                }
                RadTreeNode node3 = new RadTreeNode(((ln.client.clientTypeID == 4 || ln.client.clientTypeID == 5) ? ln.client.companyName : ((ln.client.clientTypeID == 6) ? ln.client.accountName : ln.client.surName + ", " + ln.client.otherNames)) + " - " +
                    ln.regularSusuAccountNo + " (" + amount.ToString("#,###.00") + ")",
                    ln.regularSusuAccountNo);
                node3.NavigateUrl = "/ln/regularSusu/regularSusuAccount.aspx?id=" + ln.regularSusuAccountID.ToString()
                    + "&catID=" + ln.client.categoryID.ToString();
                node3.ImageUrl = "~/images/new.jpg";
                node3.ExpandedImageUrl = "~/images/new.jpg";
                node2.Nodes.Add(node3);
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
            var years = (from l in le.regularSusuAccounts
                          where (l.client.branchID == bID)
                         select new { l.applicationDate.Year }).Distinct().OrderByDescending(p => p.Year).ToList();
            foreach (var year in years)
            {
                RadTreeNode node1 = new RadTreeNode(year.Year.ToString(), "Y:" + year.Year.ToString() + ":" + bID.ToString());
                node1.ImageUrl = "~/images/tree/folder_open.jpg";
                node1.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                rnode.Nodes.Add(node1);

                var months = (from l in le.regularSusuAccounts
                              where l.applicationDate.Year == year.Year
                                 && (l.client.branchID == bID)
                              select new { l.applicationDate.Month }).Distinct().OrderBy(p=>p.Month).ToList();
                foreach (var month in months)
                {
                    RadTreeNode node2 = new RadTreeNode(MonthName(month.Month), "M:" + month.Month.ToString() + ":" + bID.ToString());
                    node2.ImageUrl = "~/images/tree/folder_open.jpg";
                    node2.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                    node1.Nodes.Add(node2);

                    var days = (from l in le.regularSusuAccounts
                                where l.applicationDate.Year == year.Year
                                      && l.applicationDate.Month == month.Month
                                      && (l.client.branchID == bID) 
                                select new { Date=DbFunctions.TruncateTime(l.applicationDate) }).Distinct().OrderBy(p=>p.Date).ToList();
                    foreach (var day in days)
                    {
                        RadTreeNode node3 = new RadTreeNode(day.Date.Value.Day.ToString(), "D:" + day.Date.Value.ToString("yyyyMMdd") + ":" + bID.ToString());
                        node3.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                        node3.ImageUrl = "~/images/tree/folder_open.jpg";
                        node3.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                        node2.Nodes.Add(node3);
                    }
                }
            }
        }

        private string MonthName(int month)
        {
            string monthName = "";
            switch (month)
            {
                case 1:
                    monthName = "Jan";
                    break;
                case 2:
                    monthName = "Feb";
                    break;
                case 3:
                    monthName = "Mar";
                    break;
                case 4:
                    monthName = "Apr";
                    break;
                case 5:
                    monthName = "May";
                    break;
                case 6:
                    monthName = "Jun";
                    break;
                case 7:
                    monthName = "Jul";
                    break;
                case 8:
                    monthName = "Aug";
                    break;
                case 9:
                    monthName = "Sep";
                    break;
                case 10:
                    monthName = "Oct";
                    break;
                case 11:
                    monthName = "Nov";
                    break;
                case 12:
                    monthName = "Dec";
                    break;
            }

            return monthName;
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            string otherNames = this.txtOthernames.Text.Trim().Replace(";", "");
            var surName = txtSurname.Text.Trim().Replace(";", "");
            int? catID = -1;
            if (categoryID != null && categoryID.Trim() != "") catID = int.Parse(categoryID);
            List<coreLogic.client> clients = null;
            if (surName.Length > 0 && otherNames.Length > 0 && txtAccNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.surName.Contains(surName) || p.accountName.Contains(surName)).Where(
                    p => p.otherNames.Contains(otherNames)).Where(
                    p => p.accountNumber.Contains(txtAccNo.Text.Trim())).ToList();
            else if (surName.Length == 0 && otherNames.Length > 0 && txtAccNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.otherNames.Contains(otherNames)).Where(
                    p => p.accountNumber.Contains(txtAccNo.Text.Trim())).ToList();
            else if (surName.Length > 0 && otherNames.Length == 0 && txtAccNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.surName.Contains(surName) || p.accountName.Contains(surName)).Where(
                    p => p.accountNumber.Contains(txtAccNo.Text.Trim())).ToList();
            else if (surName.Length > 0 && otherNames.Length > 0 && txtAccNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(otherNames)).Where(
                    p => p.surName.Contains(surName) || p.accountName.Contains(surName)).ToList();
            else if (surName.Length == 0 && otherNames.Length == 0 && txtAccNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.accountNumber.Contains(txtAccNo.Text.Trim())).ToList();
            else if (surName.Length > 0 && otherNames.Length == 0 && txtAccNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.surName.Contains(surName) || p.accountName.Contains(surName)).ToList();
            else if (surName.Length == 0 && otherNames.Length > 0 && txtAccNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(otherNames)).ToList();
            else
                clients = le.clients.ToList();
            for (var i = clients.Count - 1; i >= 0;i--)
            {
                var cl = clients[i]; 
                if (cl.clientTypeID == 6)
                {
                    cl.surName = cl.accountName;
                    cl.otherNames = "";
                }
                else if(cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5){
                    cl.surName = cl.companyName;
                    cl.otherNames = "";
                } 
                if (cl.regularSusuAccounts.Count()==0)
                {
                    clients.Remove(cl);
                }
            }
            grid.DataSource = clients;
            grid.DataBind();
        }

        protected void grid_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
            string clientID = dataItem.KeyValues.Split(':')[1].Replace("\"", "").Replace("{", "").Replace("}", "");
            int cID = int.Parse(clientID);
            var cl = le.clients.FirstOrDefault(p => p.clientID == cID);
            if (cl != null)
            {
                var list = cl.regularSusuAccounts.ToList();

                foreach (var sa in list)
                {
                    var contrib = sa.regularSusuContributions.ToList();
                    sa.amountEntitled = ((contrib.Count > 0) ? contrib.Sum(p => p.amount) : 0);
                    sa.netAmountEntitled = sa.amountEntitled - sa.amountTaken;
                }

                e.DetailTableView.DataSource = list;
            }
        }

        protected void tree_NodeExpand(object sender, RadTreeNodeEventArgs e)
        {
            if (e.Node.Value.StartsWith("b"))
            {
                RenderTreeBranch(e.Node);
            }
            else
            {
                RenderTree(e.Node);
            }
        }

        [WebMethod]
        public static AutoCompleteBoxData GetClientSurNames(object context)
        {
            var le = new coreLogic.coreLoansEntities();
            string searchString = ((Dictionary<string, object>)context)["Text"].ToString();
            var data = (from c in le.clients
                        where c.surName.Contains(searchString)
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
            string searchString = ((Dictionary<string, object>)context)["Text"].ToString();
            var data = (from c in le.clients
                        where c.otherNames.Contains(searchString)
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
 
    }
}