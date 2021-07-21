using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.cashier
{
    public partial class _default2 : System.Web.UI.Page
    {
        string catID="1";
        coreLogic.coreLoansEntities le;

        protected void Page_Load(object sender, EventArgs e)
        {
            catID = Request.Params["catID"];
            if (catID == null) catID = "1";
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
            RadTreeNode node = null;

            var years = (from l in le.loans
                                    where ((catID != "5" && l.loanTypeID != 6) || (catID == "5" && l.loanTypeID == 6))
                         select new { l.applicationDate.Year }).Distinct().ToList();
            foreach (var year in years)
            {
                RadTreeNode node1 = new RadTreeNode(year.Year.ToString(), "Y:" + year.Year.ToString());
                rnode.Nodes.Add(node1);
                ;
                var months = (from l in le.loans
                              where l.applicationDate.Year == year.Year
                                    && ((catID != "5" && l.loanTypeID != 6) || (catID == "5" && l.loanTypeID == 6))
                              select new { l.applicationDate.Month }).Distinct().ToList();
                foreach (var month in months)
                {
                    RadTreeNode node2 = new RadTreeNode(MonthName(month.Month), "M:" + month.Month.ToString());
                    node1.Nodes.Add(node2);

                    var lns = (from l in le.loans
                               where l.applicationDate.Year == year.Year
                                    && ((catID != "5" && l.loanTypeID != 6) || (catID == "5" && l.loanTypeID == 6))
                                     && l.applicationDate.Month == month.Month
                               select l).ToList();
                    foreach (var ln in lns)
                    {
                        //ln.clientReference.Load();
                        var amount = (ln.amountApproved > 0) ? ln.amountApproved : ln.amountRequested;
                        RadTreeNode node3 = new RadTreeNode(((ln.client.clientTypeID==4||ln.client.clientTypeID==5)?ln.client.companyName:ln.client.surName+ "," + ln.client.otherNames) + " - " +
                            ln.loanNo + " (" + amount.ToString("#,###.00") + ")",
                            ln.loanNo); 
                        node2.Nodes.Add(node3);

                        RadTreeNode node4 = new RadTreeNode("Receipt",
                            ln.loanNo + "_R");
                        node4.NavigateUrl = "/ln/cashier/receipt.aspx?id=" + ln.loanID.ToString();
                        node4.Target = "_blank";
                        node3.Nodes.Add(node4);

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
            List<coreLogic.client> clients = null;
            if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccNo.Text.Trim())).Where(p => (catID != "5" && p.categoryID!=5) || (catID == "5" && p.categoryID == 5)).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccNo.Text.Trim())).Where(p => (catID != "5" && p.categoryID!=5) || (catID == "5" && p.categoryID == 5)).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccNo.Text.Trim())).Where(p => (catID != "5" && p.categoryID!=5) || (catID == "5" && p.categoryID == 5)).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(p => (catID != "5" && p.categoryID!=5) || (catID == "5" && p.categoryID == 5)).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.accountNumber.Contains(txtAccNo.Text.Trim())).Where(p => (catID != "5" && p.categoryID!=5) || (catID == "5" && p.categoryID == 5)).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(p => (catID != "5" && p.categoryID!=5) || (catID == "5" && p.categoryID == 5)).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(p => (catID != "5" && p.categoryID!=5) || (catID == "5" && p.categoryID == 5)).ToList();
            else
                clients = le.clients.Where(p => (catID != "5" && p.categoryID!=5) || (catID == "5" && p.categoryID == 5)).ToList();
            foreach (var cl in clients)
            {
                //cl.loans.Load();
            }
            grid.DataSource = clients;
            grid.DataBind();
        }

        protected void grid_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem; 
            string clientID = dataItem.KeyValues.Split(':')[1].Replace("\"","").Replace("{","").Replace("}","");
            int cID = int.Parse(clientID);
            var cl = le.clients.FirstOrDefault(p => p.clientID == cID);
            if (cl != null)
            {
                //cl.loans.Load();
                e.DetailTableView.DataSource = cl.loans.Where(p => (p.loanStatusID != 7) && ((p.amountDisbursed == 0) || (p.balance > 0) || (p.processingFeeBalance>0))).ToList();
            }
        }
    }
}