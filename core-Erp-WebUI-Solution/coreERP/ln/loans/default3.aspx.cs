using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.loans
{
    public partial class _default3 : System.Web.UI.Page
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
            var rnode = new RadTreeNode("Clients", "_root_");
            tree.Nodes.Add(rnode);
            rnode.Expanded = true;
            RadTreeNode node = null;
            List<int?> ids = new List<int?>();

            var years = (from l in le.invoiceLoans
                         select new { l.invoiceDate.Year }).Distinct().ToList();
            foreach (var year in years)
            {
                RadTreeNode node1 = new RadTreeNode(year.Year.ToString(), "Y:" + year.Year.ToString());
                rnode.Nodes.Add(node1);
                ;
                var months = (from l in le.invoiceLoans
                              where l.invoiceDate.Year == year.Year
                              select new { l.invoiceDate.Month }).Distinct().ToList();
                foreach (var month in months)
                {
                    RadTreeNode node2 = new RadTreeNode(MonthName(month.Month), "M:" + month.Month.ToString());
                    node1.Nodes.Add(node2);
                    
                    var lns = (from l in le.invoiceLoans
                                  where l.invoiceDate.Year == year.Year
                                        && l.invoiceDate.Month == month.Month
                                  select l).ToList();
                    foreach (var ln in lns)
                    {
                        if (ln.invoiceLoanMasterID == null || ids.Contains(ln.invoiceLoanMasterID) == false)
                        {
                            ids.Add(ln.invoiceLoanMasterID);
                            //ln.clientReference.Load();
                            //ln.invoiceLoanMasterReference.Load();
                            var amount = (ln.amountDisbursed > 0) ? ln.amountDisbursed : ln.proposedAmount;
                            RadTreeNode node3 = new RadTreeNode(((ln.client.clientTypeID == 4 || ln.client.clientTypeID == 5) ? ln.client.companyName : ln.client.surName + "," + ln.client.otherNames) + " - " +
                                " (" + amount.ToString("#,###.00") + ")",
                                ln.invoiceLoanID.ToString());
                            node3.NavigateUrl = "/ln/loans/invoiceLoan.aspx?" + ((ln.invoiceLoanMasterID == null) ?
                                "id=" + ln.invoiceLoanID.ToString() : "ilm=" + ln.invoiceLoanMasterID.ToString());
                            node2.Nodes.Add(node3);
                        }
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
                    p => p.accountNumber.Contains(txtAccNo.Text.Trim())
                    ).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccNo.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccNo.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.accountNumber.Contains(txtAccNo.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).ToList();
            else
                clients = le.clients.ToList();
            for (int i=clients.Count-1; i>=0; i--)
            {
                var cl = clients[i];
                //cl.invoiceLoans.Load();
                if (cl.invoiceLoans.Count() == 0)
                    clients.Remove(cl);
            }
            grid.DataSource = clients;
            grid.DataBind();
        }

        protected void grid_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                e.Canceled = true;
                GridDataItem dataItem = (GridDataItem) e.Item;
                if (dataItem.OwnerTableView.DataKeyNames[0].ToLower() == "invoiceloanid")
                {
                    string invLId = dataItem.KeyValues.Split(':')[1].Replace("\"", "").Replace("{", "").Replace("}", "");
                    int invoiceLoanId = int.Parse(invLId);


                }
            }
        }

        protected void grid_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
            if (dataItem.OwnerTableView.DataKeyNames[0].ToLower() == "clientid")
            {
                string clientID = dataItem.KeyValues.Split(':')[1].Replace("\"", "").Replace("{", "").Replace("}", "");
                int cID = int.Parse(clientID);
                var cl = le.clients.FirstOrDefault(p => p.clientID == cID);
                if (cl != null)
                {
                    //cl.invoiceLoanMasters.Load();
                    foreach (var r in cl.invoiceLoanMasters)
                    {
                        //r.clientReference.Load();
                    }
                    e.DetailTableView.DataSource = cl.invoiceLoanMasters;
                }
            }
            else
            {
                string masterID = dataItem.KeyValues.Split(':')[1].Replace("\"", "").Replace("{", "").Replace("}", "");
                int mid = int.Parse(masterID);
                var ilm = le.invoiceLoanMasters.FirstOrDefault(p => p.invoiceLoanMasterID == mid);
                if (ilm != null)
                {
                    //ilm.invoiceLoans.Load();
                    foreach (var r in ilm.invoiceLoans)
                    { 
                        //r.clientReference.Load();
                    }
                    e.DetailTableView.DataSource = ilm.invoiceLoans;
                }
            }
        }
    }
}