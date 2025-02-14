﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.investment
{
    public partial class _default : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        private int serialNumber = 0;

        public string GetSerialNumber()
        {
            serialNumber = serialNumber + 1;

            return serialNumber.ToString("#,##0");
        }

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

            var years = (from l in le.investments
                         select new { l.firstinvestmentDate.Year }).Distinct().ToList();
            foreach (var year in years)
            {
                RadTreeNode node1 = new RadTreeNode(year.Year.ToString(), "Y:" + year.Year.ToString());
                rnode.Nodes.Add(node1);
                ;
                var months = (from l in le.investments
                              where l.firstinvestmentDate.Year == year.Year
                              select new { l.firstinvestmentDate.Month }).Distinct().ToList();
                foreach (var month in months)
                {
                    RadTreeNode node2 = new RadTreeNode(MonthName(month.Month), "M:" + month.Month.ToString());
                    node1.Nodes.Add(node2);
                    
                    var lns = (from l in le.investments
                                  where l.firstinvestmentDate.Year == year.Year
                                        && l.firstinvestmentDate.Month == month.Month
                                  select l).ToList();
                    foreach (var ln in lns)
                    {
                        //ln.clientReference.Load();
                        var amount = ln.amountInvested;
                        RadTreeNode node3 = new RadTreeNode(((ln.client.clientTypeID == 4 || ln.client.clientTypeID == 5) ? ln.client.companyName : ((ln.client.clientTypeID == 6) ? ln.client.accountName : ln.client.surName + "," + ln.client.otherNames)) + " - " +
                            ln.investmentNo + " (" + amount.ToString("#,###.00") + ")",
                            ln.investmentNo);
                        node3.NavigateUrl = "/ln/investment/investment.aspx?id=" + ln.investmentID.ToString();
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
            List<coreLogic.client> clients = null;
            if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccNo.Text.Trim().Length > 0)
                clients = le.clients
                    .Where(p => p.clientTypeID == 3)
                    .Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccNo.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccNo.Text.Trim().Length > 0)
                clients = le.clients
                    .Where(p => p.clientTypeID == 3)
                    .Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccNo.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccNo.Text.Trim().Length > 0)
                clients = le.clients
                    .Where(p => p.clientTypeID == 3)
                    .Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccNo.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccNo.Text.Trim().Length == 0)
                clients = le.clients
                    .Where(p => p.clientTypeID == 3)
                    .Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccNo.Text.Trim().Length > 0)
                clients = le.clients
                    .Where(p => p.clientTypeID == 3)
                    .Where(p => p.accountNumber.Contains(txtAccNo.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccNo.Text.Trim().Length == 0)
                clients = le.clients
                    .Where(p => p.clientTypeID == 3)
                    .Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).ToList();
            else
                clients = le.clients
                    .Where(p => p.clientTypeID == 3)
                    .ToList();
            for(int i=clients.Count-1;i>=0;i--){
                var cl = clients[i];
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
                if (cl.investments.Count == 0) clients.Remove(cl);
            }
            grid.DataSource = clients;
            grid.DataBind();
        }

        protected void grid_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
            string accountNumber = dataItem["accountNumber"].Text;
            string clientID = dataItem.KeyValues.Split(':')[1].Replace("\"","").Replace("{","").Replace("}","");
            int cID = int.Parse(clientID);
            var cl = le.clients.FirstOrDefault(p => p.clientID == cID);
            if (cl != null)
            { 
                e.DetailTableView.DataSource = cl.investments;
            }
        }
    }
}