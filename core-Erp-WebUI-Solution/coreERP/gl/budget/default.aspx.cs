using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreLogic;
using System.Data;
using Telerik.Web.UI;
using System.Collections; 

namespace coreERP.gl.budget
{
    public partial class _default : corePage
    {
        public override string URL
        {
            get { return "~/gl/budget/default.aspx"; }
        }
           
        core_dbEntities ent;
        jnl_batch_tmp batch;
        jnl_batch batchPost;
        bool doubleClickFlag = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ent = new core_dbEntities(); 
                RadTreeView1.NodeClick += new RadTreeViewEventHandler(RadTreeView1_NodeClick);
                if (!IsPostBack)
                {
                    cboCC.Items.Add(new RadComboBoxItem("All Cost Centers", ""));
                    foreach (var ou in ent.gl_ou.OrderBy(p => p.ou_name))
                    {
                        cboCC.Items.Add(new RadComboBoxItem(ou.ou_name, ou.ou_id.ToString()));
                    }
                    RenderTree();
                }
            }
            catch (Exception x){
                ManageException(x);
            }
        }

        public void RadTreeView1_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            try
            {
                var startDate = DateTime.ParseExact(e.Node.ParentNode.Value.Split(':')[1],
                    "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                int? ccID = null;
                var cc = e.Node.Value.Split(':')[1];
                if (cc != "")
                {
                    ccID = int.Parse(cc);
                }
                Edit(startDate, ccID); 
            }
            catch (Exception x)
            {
                ManageException(x);
            }
        }
        
        private void RenderTree()
        {
            try
            {
                this.RadTreeView1.Nodes.Clear();
                RadTreeNode rootNode = new RadTreeNode("The Company", "__root__");
                rootNode.ImageUrl = "~/images/tree/folder_open.jpg";
                rootNode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                this.RadTreeView1.Nodes.Add(rootNode);

                var years = (from j in ent.budgets 
                             select new { j.monthEndDate.Year }).Distinct().OrderByDescending(p => p.Year);
                foreach (var year in years)
                {
                    RadTreeNode node = rootNode.Nodes.FindNode(p => p.Value == "y:" + year.Year.ToString());
                    if (node == null)
                    {
                        node = new RadTreeNode(year.Year.ToString(), "y:" + year.Year.ToString());
                        node.Visible = true;
                        node.ImageUrl = "~/images/tree/folder_open.jpg";
                        node.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                        node.ToolTip = "Expand to see transactions for year: " + year.ToString();
                        rootNode.Nodes.Add(node);
                    }
                    var months = (from j in ent.budgets
                                  where j.monthEndDate.Year==year.Year
                                  select new { j.monthEndDate.Month }).Distinct().OrderByDescending(p => p.Month);
                    foreach (var month in months)
                    {
                        var monthName = MonthName(month.Month);
                        RadTreeNode childnode = node.Nodes.FindNode(p => p.Value == "m:" + year.Year.ToString() +
                            "," + month.Month.ToString());
                        if (childnode == null)
                        {
                            childnode = new RadTreeNode(monthName, "m:" + year.Year.ToString() +
                                "," + month.Month.ToString());
                            childnode.Visible = true;
                            childnode.ImageUrl = "~/images/tree/folder_open.jpg";
                            childnode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                            childnode.ToolTip = "Expand to see transactions for month: " + year.Year.ToString() +
                                ", " + monthName;
                            childnode.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                            node.Nodes.Add(childnode);
                        }
                    }
                }                 
                rootNode.Expanded = true;

            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        private void RenderTreeMonth(RadTreeNode childnode)
        {
            try
            {
                var split = childnode.Value.Split(':')[1];
                var split2 = split.Split(',');
                var year = int.Parse(split2[0]);
                var month = int.Parse(split2[1]);
                var days = (from jnl in ent.budgets
                            where jnl.monthEndDate.Year == year
                                && jnl.monthEndDate.Month == month
                            select new { jnl.monthEndDate.Day }).Distinct().OrderByDescending(p => p.Day);
                foreach (var day in days)
                {
                    RadTreeNode gchildnode = new RadTreeNode(day.Day.ToString(), "d:" + year.ToString() +
                            month.ToString().PadLeft(2, '0') + day.Day.ToString().PadLeft(2, '0'));
                    gchildnode.Visible = true;
                    gchildnode.ImageUrl = "~/images/tree/folder_open.jpg";
                    gchildnode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                    gchildnode.ToolTip = "Expand to see transactions for date: " +
                        (new DateTime(year, month, day.Day)).ToString("dd-MMM-yyyy");
                    gchildnode.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                    childnode.Nodes.Add(gchildnode);
                } 
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        private void RenderTreeDay(RadTreeNode gchildnode)
        {
            try
            {
                var prof = ent.comp_prof.SingleOrDefault();
                var enforceOUSec = (prof == null) ? false : prof.enf_ou_sec;
                var startDate = DateTime.ParseExact(gchildnode.Value.Split(':')[1], "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                var endDate = startDate.AddDays(1).AddSeconds(-1);
                var batches = (from jnl in ent.budgets 
                               where  jnl.monthEndDate >= startDate
                                   && jnl.monthEndDate <= endDate 
                               select jnl.cost_center_id).Distinct();
                foreach (var b in batches)
                {
                    var cc = ent.gl_ou.FirstOrDefault(p => p.ou_id == b);
                    var ccname = (cc == null) ? "All Cost Centers" : cc.ou_name;
                    RadTreeNode gchildnode2 = new RadTreeNode(ccname, "c:" + ((b == null) ? "" : b.ToString()) + ":u");
                    gchildnode2.Visible = true; 
                    gchildnode2.ImageUrl = "~/images/editNew.jpg";
                    gchildnode2.ExpandedImageUrl = "~/images/editNew.jpg"; 
                    gchildnode2.ToolTip = "Click to see Budget for cost center: " + ccname; 
                    gchildnode2.PostBack = true;
                    gchildnode.Nodes.Add(gchildnode2);
                } 
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        private string MonthName(int month)
        {
            string monthName = "";
            switch (month)
            {
                case 1:
                    monthName = "January";
                    break;
                case 2:
                    monthName = "February";
                    break;
                case 3:
                    monthName = "March";
                    break;
                case 4:
                    monthName = "April";
                    break;
                case 5:
                    monthName = "May";
                    break;
                case 6:
                    monthName = "June";
                    break;
                case 7:
                    monthName = "July";
                    break;
                case 8:
                    monthName = "August";
                    break;
                case 9:
                    monthName = "September";
                    break;
                case 10:
                    monthName = "October";
                    break;
                case 11:
                    monthName = "November";
                    break;
                case 12:
                    monthName = "December";
                    break;
                default:
                    monthName = "";
                    break;
            }

            return monthName;
        }
          
        private string toPlural(string item)
        {
            var rtr = item;
            var lastDigit = item.Substring(item.Length - 1, 1);
            var lastDigit2 = item.Substring(item.Length - 2, 2);
            if (lastDigit == "s" || lastDigit2 == "es") { }
            else if (lastDigit == "y" || lastDigit == "i")
            {
                rtr = item.Substring(0, item.Length - 1);
            }
            else if (lastDigit2 == "ch" || lastDigit2 == "sh")
            {
                rtr = item + "es";
            }
            else
            {
                rtr = item + "s";
            }
            return rtr;
        }

        private void ManageException(Exception ex)
        {
            string errorMsg = "There was an error processing your request:";
            if (ex is System.Data.Entity.Core.UpdateException)
            {
                if (ex.InnerException.Message.Contains("uk_acct_cat_name") ||
                    ex.InnerException.Message.Contains("uk_acct_cat_code"))
                {
                    errorMsg += "<br />The Main Account Head you are trying to create already exist.";
                }
                if (ex.InnerException.Message.Contains("uk_acct_cat_max_acct_num") ||
                    ex.InnerException.Message.Contains("uk_acct_cat_min_acct_num"))
                {
                    errorMsg += "<br />The Account number range specified overlaps another account head.";
                }
            }
            errorMsg += "Please correct and continue or cancel."; 
        }
         
        protected void RadTreeView1_ContextMenuItemClick(object sender, RadTreeViewContextMenuEventArgs e)
        {
            try
            {
                RadTreeNode clickedNode = e.Node;
                core_dbEntities ent = new core_dbEntities();

                switch (e.MenuItem.Value)
                {
                    case "New":
                        var itemType = clickedNode.Value.Split(':')[0];
                        AddNew();
                        break;
                    case "Delete":
                        itemType = clickedNode.Value.Split(':')[0];
                        if (itemType == "c")
                        {
                            var startDate = DateTime.ParseExact(e.Node.ParentNode.Value.Split(':')[1],
                                "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                            int? ccID = null;
                            var cc = e.Node.Value.Split(':')[1];
                            if (cc != "")
                            {
                                ccID = int.Parse(cc);
                            }
                            var eomDate = (new DateTime(startDate.Year, startDate.Month, 1)).AddMonths(1).AddSeconds(-1);
                            var budgets = ent.budgets.Where(p => p.monthEndDate == eomDate && p.cost_center_id == ccID).ToList();
                            for (int i = budgets.Count - 1; i >= 0; i--)
                            {
                                var b = budgets[i];
                                ent.budgets.Remove(b);
                            }
                            ent.SaveChanges();
                            RadTreeView1.Nodes.Clear();
                            RenderTree();
                            plnEdit.Visible = false;
                        }
                        break;
                    case "Edit":
                        itemType = clickedNode.Value.Split(':')[0];
                        if (itemType == "c")
                        {
                            var startDate = DateTime.ParseExact(e.Node.ParentNode.Value.Split(':')[1], 
                                "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                            int? ccID = null;
                            var cc = e.Node.Value.Split(':')[1];
                            if (cc != "")
                            {
                                ccID = int.Parse(cc);
                            }
                            Edit(startDate, ccID); 
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
       
        protected void RadTreeView1_NodeExpand(object sender, RadTreeNodeEventArgs e)
        {
            if (e.Node.Value.StartsWith("m"))
            {
                RenderTreeMonth(e.Node);
            }
            else if (e.Node.Value.StartsWith("d"))
            {
                RenderTreeDay(e.Node);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (dtpDate.SelectedDate != null)
            {
                int? ccID = null;
                if (cboCC.SelectedValue != "")
                {
                    ccID = int.Parse(cboCC.SelectedValue);
                }
                var eomDate = dtpDate.SelectedDate.Value;
                eomDate = (new DateTime(eomDate.Year, eomDate.Month, 1)).AddMonths(1).AddSeconds(-1); 
                Session["eomDate"] = eomDate;
                Session["ccID"] = ccID;

                var budgetsBS = ent.budgets.Where(p => p.monthEndDate == eomDate &&
                        (p.cost_center_id == ccID || p.cost_center_id == null)
                        && p.acct.acct_heads.acct_cats.cat_code <= 3).ToList();
                var budgetsIS = ent.budgets.Where(p => p.monthEndDate == eomDate && 
                        (p.cost_center_id == ccID || p.cost_center_id == null)
                        && p.acct.acct_heads.acct_cats.cat_code > 3).ToList();
                bool foundBS = true;
                bool foundIS = true;
                if (budgetsBS.Count == 0)
                {
                    budgetsBS = new List<coreLogic.budget>();
                    foundBS = false;
                }
                if (budgetsIS.Count == 0)
                {
                    budgetsIS = new List<coreLogic.budget>();
                    foundIS = false;
                }

                foreach (RepeaterItem item in rpBudgetBS.Items)
                {
                    var txtAmount = item.FindControl("txtAmount") as RadNumericTextBox;
                    var lblAcctID = item.FindControl("lblAcctID") as Label;
                    if (txtAmount != null && lblAcctID != null && lblAcctID.Text != "")
                    {
                        var amt = (txtAmount.Value == null) ? 0 : txtAmount.Value.Value;
                        var acc_id = int.Parse(lblAcctID.Text);
                        if (foundBS == false)
                        {
                            var b = new coreLogic.budget
                            {
                                acct_id = acc_id,
                                budgetAmount = amt,
                                monthEndDate = eomDate,
                                cost_center_id = ccID,
                                creationDate = DateTime.Now,
                                creator = User.Identity.Name
                            };
                            ent.budgets.Add(b);
                        }
                        else
                        {
                            var b = ent.budgets.FirstOrDefault(p => p.monthEndDate == eomDate &&
                                     (p.cost_center_id == ccID || p.cost_center_id == null)
                                    && p.acct_id == acc_id);
                            if (b != null)
                            {
                                b.budgetAmount = amt;
                            }
                        }
                    }
                }

                foreach (RepeaterItem item in rpBudgetIS.Items)
                {
                    var txtAmount = item.FindControl("txtAmount") as RadNumericTextBox;
                    var lblAcctID = item.FindControl("lblAcctID") as Label;
                    if (txtAmount != null && lblAcctID != null && lblAcctID.Text != "")
                    {
                        var amt = (txtAmount.Value == null) ? 0 : txtAmount.Value.Value;
                        var acc_id = int.Parse(lblAcctID.Text);
                        if (foundIS == false)
                        {
                            var b = new coreLogic.budget
                            {
                                acct_id = acc_id,
                                budgetAmount = amt,
                                monthEndDate = eomDate,
                                cost_center_id = ccID,
                                creationDate = DateTime.Now,
                                creator = User.Identity.Name
                            };
                            ent.budgets.Add(b);
                        }
                        else
                        {
                            var b = ent.budgets.FirstOrDefault(p => p.monthEndDate == eomDate &&
                                     (p.cost_center_id == ccID || p.cost_center_id == null)
                                    && p.acct_id == acc_id);
                            if (b != null)
                            {
                                b.budgetAmount = amt;
                            }
                        }
                    }
                }

                ent.SaveChanges();
                HtmlHelper.MessageBox("Budget Successfully Saved", "coreERP: Success", IconType.ok);
                rpBudgetBS.DataSource = new List<coreLogic.budget>();
                rpBudgetBS.DataBind();
                rpBudgetIS.DataSource = new List<coreLogic.budget>();
                rpBudgetIS.DataBind();
                dtpDate.SelectedDate=null;
                plnEdit.Visible = false;
                RadTreeView1.Nodes.Clear();
                RenderTree();
            }
        }

        protected void dtpDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            OnChange();
        }

        public string GetAccountNumber(object id)
        {
            var num = "";

            if (id != null)
            {
                var acc_id = int.Parse(id.ToString());
                var acc = ent.accts.FirstOrDefault(p => p.acct_id == acc_id);
                if (acc != null)
                {
                    num = acc.acc_num;
                }
            }

            return num;
        }

        public string GetAccountName(object id)
        {
            var name = "";

            if (id != null)
            {
                var acc_id = int.Parse(id.ToString());
                var acc = ent.accts.FirstOrDefault(p => p.acct_id == acc_id);
                if (acc != null)
                {
                    name = acc.acc_name;
                }
            }

            return name;
        }

        protected void OnChange()
        {
            if (dtpDate.SelectedDate != null)
            {
                int? ccID = null;
                if (cboCC.SelectedValue != "")
                {
                    ccID = int.Parse(cboCC.SelectedValue);
                }
                var eomDate = dtpDate.SelectedDate.Value;
                eomDate = (new DateTime(eomDate.Year, eomDate.Month, 1)).AddMonths(1).AddSeconds(-1);
                dtpDate.SelectedDate = eomDate;
                Session["eomDate"] = eomDate;
                Session["ccID"] = ccID;

                var budgetBS = ent.budgets.Where(p => p.monthEndDate == eomDate &&
                        (p.cost_center_id == ccID || p.cost_center_id == null)
                    && p.acct.acct_heads.acct_cats.cat_code <= 3).ToList();
                var budgetIS = ent.budgets.Where(p => p.monthEndDate == eomDate &&
                        (p.cost_center_id == ccID || p.cost_center_id == null)
                    && p.acct.acct_heads.acct_cats.cat_code > 3).ToList();
                if (budgetBS.Count == 0)
                {
                    budgetBS = new List<coreLogic.budget>();
                    var accs = ent.vw_accounts.Where(p => p.cat_code <= 3).OrderBy(p => p.acc_num).ToList();
                    foreach (var acc in accs)
                    {
                        var b = new coreLogic.budget
                        {
                            acct_id = acc.acct_id,
                            monthEndDate = eomDate,
                            budgetAmount = 0
                        };
                        budgetBS.Add(b);
                    }
                }
                if (budgetIS.Count == 0)
                {
                    budgetIS = new List<coreLogic.budget>();
                    var accs = ent.vw_accounts.Where(p => p.cat_code > 3).OrderBy(p => p.acc_num).ToList();
                    foreach (var acc in accs)
                    {
                        var b = new coreLogic.budget
                        {
                            acct_id = acc.acct_id,
                            monthEndDate = eomDate,
                            budgetAmount = 0
                        };
                        budgetIS.Add(b);
                    }
                }

                rpBudgetBS.DataSource = budgetBS;
                rpBudgetBS.DataBind();

                rpBudgetIS.DataSource = budgetIS;
                rpBudgetIS.DataBind();

                var b2 = budgetBS.FirstOrDefault();
                if (b2 == null)
                {
                    b2 = budgetIS.FirstOrDefault();
                }
                if (b2 != null && b2.cost_center_id != null)
                {
                    cboCC.SelectedValue = b2.cost_center_id.ToString();
                }
                else if (b2 != null)
                {
                    cboCC.SelectedValue = "";
                }
            }
        }

        protected void cboCC_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            OnChange();
        }

        protected void AddNew()
        {
            plnEdit.Visible = true;
            dtpDate.SelectedDate = null;
            cboCC.SelectedValue = "";
        }

        protected void Edit(DateTime eomDate, int? ccID)
        {
            plnEdit.Visible = true;
            dtpDate.SelectedDate = eomDate;
            if (ccID != null)
            {
                cboCC.SelectedValue = ccID.ToString();
            }
            else
            {
                cboCC.SelectedValue = "";
            }
            OnChange();
        }
    }
}
