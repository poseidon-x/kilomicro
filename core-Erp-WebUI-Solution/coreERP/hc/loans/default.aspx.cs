using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;

namespace coreERP.hc.loans
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
            tree.Nodes.Clear();
            RadTreeNode node = new RadTreeNode("Staff Loans", "__root__");
            node.ImageUrl = "~/images/tree/folder_open.jpg";
            node.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
            node.Expanded = true;
            tree.Nodes.Add(node);

            var years = (
                    from l in le.staffLoans
                    select new { l.creationDate.Year }
                ).Distinct().OrderByDescending(p => p.Year);
            foreach (var r in years)
            {
                RadTreeNode node2 = new RadTreeNode(r.Year.ToString(), "y:" + r.Year.ToString());
                node2.ToolTip = "Expand to see all staff loans for the year " + r.Year.ToString();
                node2.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                node2.ImageUrl = "~/images/tree/folder_open.jpg";
                node2.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                node.Nodes.Add(node2);
            }
        }

        private void RenderTreeYear(RadTreeNode node2)
        {
            var year = int.Parse(node2.Value.Split(':')[1]);
            var months = (
                    from l in le.staffLoans
                    where l.creationDate.Year==year
                    select new { l.creationDate.Month }
                ).Distinct().OrderByDescending(p => p.Month);
            foreach (var r in months)
            {
                RadTreeNode node3 = new RadTreeNode(MonthName(r.Month), "m:" + year.ToString() + "," + r.Month.ToString());
                node3.ToolTip = "Expand to see all staff loans for the month " + MonthName(r.Month) + " " + year.ToString();
                node3.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                node3.ImageUrl = "~/images/tree/folder_open.jpg";
                node3.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                node2.Nodes.Add(node3);
            }
        }

        private void RenderTreeMonth(RadTreeNode node2)
        {
            var split = node2.Value.Split(':')[1];
            var split2 = split.Split(',');
            var year = int.Parse(split2[0]);
            var month = int.Parse(split2[1]);
            var days = (
                    from l in le.staffLoans
                    where l.creationDate.Year == year
                        && l.creationDate.Month==month
                    select new { Date = DbFunctions.TruncateTime(l.creationDate) }
                ).Distinct().OrderByDescending(p => p.Date);
            foreach (var r in days)
            {
                RadTreeNode node3 = new RadTreeNode(r.Date.Value.Day.ToString(), 
                    "d:" + year.ToString() + month.ToString().PadLeft(2,'0') + r.Date.Value.Day.ToString().PadLeft(2,'0'));
                node3.ToolTip = "Expand to see all staff loans for the day " 
                    + (new DateTime(year,month, r.Date.Value.Day)).ToString("dd-MMM-yyyy");
                node3.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                node3.ImageUrl = "~/images/tree/folder_open.jpg";
                node3.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                node2.Nodes.Add(node3);
            }
        }

        private void RenderTreeDay(RadTreeNode node2)
        {
            var split = node2.Value.Split(':')[1];
            var startDate = DateTime.ParseExact(split, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            var endDate = startDate.AddDays(1).AddSeconds(-1);
            var loans = (
                    from l in le.staffLoans
                    from s in le.staffs
                    where l.staffID == s.staffID
                        && l.creationDate >= startDate
                        && l.creationDate <= endDate
                    select new { 
                        staffName = s.surName + ", "+ s.otherNames,
                        amount = l.principal,
                        balance = l.principalBalance+l.interestBalance,
                        l.staffLoanID
                    }
                ).OrderBy(p => p.staffName);
            foreach (var r in loans)
            {
                RadTreeNode node3 = new RadTreeNode(r.staffName + " | " + r.amount.ToString("#,##0.#0")
                    + " | " + r.balance.ToString("#,##0.#0"),
                    "l:" + r.staffLoanID.ToString());
                node3.ToolTip = "Click to view the details of this loan";
                node3.NavigateUrl = "~/hc/loans/loan.aspx?id=" + r.staffLoanID.ToString();
                node3.ImageUrl = "~/images/new.jpg";
                node3.ExpandedImageUrl = "~/images/new.jpg";
                node2.Nodes.Add(node3);
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
            List<coreLogic.staff> staffs = null;
            if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && this.txtAccNo.Text.Trim().Length > 0)
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
            for (var i = staffs.Count - 1; i >= 0;i--)
            {
                var cl = staffs[i];
                //cl.staffLoans.Load(); 
                foreach (var l in cl.staffLoans)
                {
                    ////l.staffReference.Load();
                }
                if (cl.staffLoans.Count() == 0)
                {
                    staffs.Remove(cl);
                }
            }
            grid.DataSource = staffs;
            grid.DataBind();
        }

        protected void grid_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
            string accountNumber = dataItem["staffNo"].Text;
            string staffID = dataItem.KeyValues.Split(':')[1].Replace("\"","").Replace("{","").Replace("}","");
            int cID = int.Parse(staffID);
            var cl = le.staffs.FirstOrDefault(p => p.staffID == cID);
            if (cl != null)
            {
                //cl.staffLoans.Load();
                foreach (var l in cl.staffLoans)
                { 
                }
                e.DetailTableView.DataSource = cl.staffLoans.ToList();
            }
        }

        protected void tree_NodeExpand(object sender, RadTreeNodeEventArgs e)
        {
            if (e.Node.Value.StartsWith("y"))
            {
                RenderTreeYear(e.Node);
            }
            else if (e.Node.Value.StartsWith("m"))
            {
                RenderTreeMonth(e.Node);
            }
            else if (e.Node.Value.StartsWith("d"))
            {
                RenderTreeDay(e.Node);
            }
        }
    }
}