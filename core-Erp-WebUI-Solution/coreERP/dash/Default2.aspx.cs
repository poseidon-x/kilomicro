using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.Data.Entity;
using coreLogic;
using System.Linq;

public partial class Default2 : System.Web.UI.Page 
{
    coreLogic.coreLoansEntities le;

    protected void Page_Load(object sender, EventArgs e)
    {
        le = new coreLogic.coreLoansEntities();
        if (!Page.IsPostBack)
        {
            cboBranch.Items.Add(new RadComboBoxItem("All Branches", ""));
            foreach (var br in le.branches.OrderBy(p => p.branchName))
            {
                cboBranch.Items.Add(new RadComboBoxItem(br.branchName, br.branchID.ToString()));
            }
            Bind();
        }
        else
        {

        }
    }

    private bool IsAdmin()
    {
        using (var ent = new coreSecurityEntities())
        {
            var user = ent.users.FirstOrDefault(p => p.user_name == User.Identity.Name);
            if (user != null)
            {
                var rl = user.user_roles.FirstOrDefault(p => p.roles.role_name == "admin");
                return rl != null;
            }
        }

        return false;
    }

    private void Bind()
    {
        int? branchID = null;
        if (Session["branchID"] != null && Session["branchID"] != "")
        {
            branchID = int.Parse(Session["branchID"].ToString());
        }
        try
        {
            var isAdmin = IsAdmin();
            var due = (from ls in le.repaymentSchedules
                       join l in le.loans on ls.loanID equals l.loanID
                       join c in le.clients on l.clientID equals c.clientID
                       join s in le.staffs on l.staffID equals s.staffID into stf
                       from st in stf.DefaultIfEmpty()
                       where ((ls.interestBalance > 1 || ls.principalBalance > 1)
                                && (ls.repaymentDate <= DateTime.Now))
                                && (l.loanStatusID != 7)
                                && (l.loanStatusID > 3)
                                && (
                                    (st != null && st.userName.Trim().ToLower() == User.Identity.Name.Trim().ToLower())
                                    || (isAdmin == true)
                                  )
                                && (branchID == null || c.branchID == branchID)
                       orderby ls.repaymentDate descending
                       select new
                       {
                           clientID = c.accountNumber,
                           clientName = c.surName + ", " + c.otherNames,
                           amountDue = ls.principalBalance + ls.interestBalance,
                           dateDue = ls.repaymentDate,
                           l.loanID,
                           staffName = ((st == null) ? "" : st.surName + ", " + st.otherNames),
                           loanNo = l.loanNo,
                       }).ToList();
            Session["due"] = due;
            gridDue.DataSource = due;
            gridDue.DataBind();
        }
        catch (Exception) { }

        try
        {
            var app = (
                        from l in le.loans
                        join c in le.clients on l.clientID equals c.clientID
                        join s in le.staffs on l.staffID equals s.staffID into stf
                        from st in stf.DefaultIfEmpty()
                        where l.loanStatusID <= 2
                                && (branchID == null || c.branchID == branchID)
                        select new
                        {
                            clientID = c.accountNumber,
                            clientName = c.surName + ", " + c.otherNames,
                            l.amountRequested,
                            l.applicationDate,
                            l.loanID,
                            c.categoryID,
                            staffName = ((st == null) ? "" : st.surName + ", " + st.otherNames),
                            loanNo = l.loanNo,
                        }).ToList();
            Session["app"] = app;
            gridApp.DataSource = app;
            gridApp.DataBind();
        }
        catch (Exception) { }


        try
        {
            var und = (
                        from l in le.loans
                        join c in le.clients on l.clientID equals c.clientID
                        join s in le.staffs on l.staffID equals s.staffID into stf
                        from st in stf.DefaultIfEmpty()
                        where l.loanStatusID == 3 && l.amountApproved > 0
                                && (branchID == null || c.branchID == branchID)
                        select new
                        {
                            clientID = c.accountNumber,
                            clientName = c.surName + ", " + c.otherNames,
                            l.amountRequested,
                            l.applicationDate,
                            l.amountApproved,
                            l.finalApprovalDate,
                            c.categoryID,
                            l.loanID,
                            staffName = ((st == null) ? "" : st.surName + ", " + st.otherNames),
                            loanNo = l.loanNo,
                        }).ToList();
            Session["und"] = und;
            gridUnd.DataSource = und;
            gridUnd.DataBind();
        }
        catch (Exception) { }

        try
        {
            var top10Borrowers =
                (from l in le.loans
                 from c in le.clients
                 join r in le.loanRepayments on l.loanID equals r.loanID into lrs
                 from lr in lrs.DefaultIfEmpty()
                 join s in le.staffs on l.staffID equals s.staffID into stf
                 from st in stf.DefaultIfEmpty()
                 where c.clientID == l.clientID && l.balance > 20
                                && (branchID == null || c.branchID == branchID)
                 group lr by new
                 {
                     lr.loanID,
                     c.accountNumber,
                     c.surName,
                     c.otherNames,
                     l.applicationDate,
                     l.finalApprovalDate,
                     l.disbursementDate,
                     l.amountApproved,
                     l.amountDisbursed,
                     c.categoryID,
                     l.amountRequested,
                     l.loanNo,
                     staffName = ((st == null) ? "" : st.surName + ", " + st.otherNames),
                 } into lrg
                 select new
                 {
                     clientID = lrg.Key.accountNumber,
                     clientName = lrg.Key.surName + ", " + lrg.Key.otherNames,
                     amountRequested = lrg.Key.amountRequested,
                     lrg.Key.applicationDate,
                     lrg.Key.amountApproved,
                     lrg.Key.finalApprovalDate,
                     lrg.Key.categoryID,
                     lrg.Key.disbursementDate,
                     lrg.Key.amountDisbursed,
                     amountPaid = lrg.Sum(p => p.principalPaid + p.interestPaid),
                     lastPaymentDate = lrg.Max(p => p.repaymentDate),
                     totalDue = lrg.Key.amountDisbursed - lrg.Sum(p => p.principalPaid + p.interestPaid),
                     loanID = lrg.Key.loanNo,
                     staffName = lrg.Key.staffName,
                     loanNo = lrg.Key.loanNo,
                 }).OrderByDescending(p => p.totalDue).Take(10).ToList();
            Session["top10Borrowers"] = top10Borrowers;
            gridTopBorrowers.DataSource = top10Borrowers;
            gridTopBorrowers.DataBind();
        }
        catch (Exception) { }

        coreReports.reportEntities rent = new coreReports.reportEntities();
        try
        {
            var disbRec = rent.getDisbRecComparison(branchID).OrderByDescending(p => p.month).Take(6).OrderBy(p => p.month).ToList();

            chComp.DataSource = disbRec;
            chComp.PlotArea.Series[0].DataFieldY = "disbursed";
            chComp.PlotArea.Series[1].DataFieldY = "paid";
            chComp.PlotArea.XAxis.DataLabelsField = "month";
        }
        catch (Exception) { }
        chComp.DataBind();
        try
        {
            var col = rent.getCollectionRatioByMonth(branchID).OrderByDescending(p => p.month).Take(5).OrderBy(p => p.month).ToList();

            chCol.DataSource = col;
            chCol.PlotArea.Series[0].DataFieldY = "collection";
            chCol.PlotArea.XAxis.DataLabelsField = "month";
            chCol.DataBind();
        }
        catch (Exception) { }
    }

    protected void gridTopBorrowers_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        gridTopBorrowers.CurrentPageIndex = e.NewPageIndex;
        var top10Borrowers = Session["top10Borrowers"];
        gridTopBorrowers.DataSource = top10Borrowers;
        gridTopBorrowers.CurrentPageIndex = e.NewPageIndex;
        gridTopBorrowers.DataBind();
        gridTopBorrowers.CurrentPageIndex = e.NewPageIndex;
    }

    protected void gridDue_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        gridDue.CurrentPageIndex = e.NewPageIndex;
        var due = Session["due"];
        gridDue.DataSource = due;
        gridDue.CurrentPageIndex = e.NewPageIndex;
        gridDue.DataBind();
        gridDue.CurrentPageIndex = e.NewPageIndex;
    }

    protected void gridApp_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        gridApp.CurrentPageIndex = e.NewPageIndex;
        var app = Session["app"];
        gridApp.DataSource = app;
        gridApp.CurrentPageIndex = e.NewPageIndex;
    }

    protected void gridUnd_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        var und = Session["und"];
        gridUnd.DataSource = und;
        gridUnd.CurrentPageIndex = e.NewPageIndex;
    }

    protected void gridTopBorrowers_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        var top10Borrowers = Session["top10Borrowers"];
        gridTopBorrowers.DataSource = top10Borrowers;
    }

    protected void gridDue_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        int? branchID = null;
        if (Session["branchID"] != null && Session["branchID"] != "")
        {
            branchID = int.Parse(Session["branchID"].ToString());
        }
        var isAdmin = IsAdmin();
        var due = (from ls in le.repaymentSchedules
                   join l in le.loans on ls.loanID equals l.loanID
                   join c in le.clients on l.clientID equals c.clientID
                   join s in le.staffs on l.staffID equals s.staffID into stf
                   from st in stf.DefaultIfEmpty()
                   where ((ls.interestBalance > 1 || ls.principalBalance > 1)
                            && (ls.repaymentDate <= DateTime.Now))
                            && (l.loanStatusID != 7)
                            && (l.loanStatusID > 3)
                            && (
                                (st != null && st.userName.Trim().ToLower() == User.Identity.Name.Trim().ToLower())
                                || (isAdmin == true)
                              )
                            && (branchID == null || c.branchID == branchID)
                   orderby ls.repaymentDate descending
                   select new
                   {
                       clientID = c.accountNumber,
                       clientName = c.surName + ", " + c.otherNames,
                       amountDue = ls.principalBalance + ls.interestBalance,
                       dateDue = ls.repaymentDate,
                       c.categoryID,
                       l.loanID,
                       staffName = ((st == null) ? "" : st.surName + ", " + st.otherNames),
                       loanNo = l.loanNo,
                   }).ToList();
        gridDue.DataSource = due; 
    }

    protected void gridApp_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        int? branchID = null;
        if (Session["branchID"] != null && Session["branchID"] != "")
        {
            branchID = int.Parse(Session["branchID"].ToString());
        }
        var app = (
                        from l in le.loans
                        join c in le.clients on l.clientID equals c.clientID
                        join s in le.staffs on l.staffID equals s.staffID into stf
                        from st in stf.DefaultIfEmpty()
                        where l.loanStatusID <= 2
                        select new
                        {
                            clientID = c.accountNumber,
                            clientName = c.surName + ", " + c.otherNames,
                            l.amountRequested,
                            l.applicationDate,
                            c.categoryID,
                            l.loanID,
                            staffName = ((st == null) ? "" : st.surName + ", " + st.otherNames),
                            loanNo = l.loanNo,
                        }).ToList();
        gridApp.DataSource = app;
    }

    protected void gridUnd_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        int? branchID = null;
        if (Session["branchID"] != null && Session["branchID"] != "")
        {
            branchID = int.Parse(Session["branchID"].ToString());
        }
        var und = (
                        from l in le.loans
                        join c in le.clients on l.clientID equals c.clientID
                        join s in le.staffs on l.staffID equals s.staffID into stf
                        from st in stf.DefaultIfEmpty()
                        where l.loanStatusID == 3
                                && (branchID == null || c.branchID == branchID)
                        select new
                        {
                            clientID = c.accountNumber,
                            clientName = c.surName + ", " + c.otherNames,
                            l.amountRequested,
                            l.applicationDate,
                            l.amountApproved,
                            l.finalApprovalDate,
                            c.categoryID,
                            l.loanID,
                            staffName = ((st == null) ? "" : st.surName + ", " + st.otherNames),
                            loanNo = l.loanNo,
                        }).ToList();
        gridUnd.DataSource = und;
    }

    protected void gridDue_Load(object sender, EventArgs e)
    {
        int? branchID = null;
        if (Session["branchID"] != null && Session["branchID"] != "")
        {
            branchID = int.Parse(Session["branchID"].ToString());
        }
        var isAdmin = IsAdmin();
        var due = (from ls in le.repaymentSchedules
                   join l in le.loans on ls.loanID equals l.loanID
                   join c in le.clients on l.clientID equals c.clientID
                   join s in le.staffs on l.staffID equals s.staffID into stf
                   from st in stf.DefaultIfEmpty()
                   where ((ls.interestBalance > 1 || ls.principalBalance > 1)
                            && (ls.repaymentDate <= DateTime.Now))
                            && (l.loanStatusID != 7)
                            && (l.loanStatusID > 3)
                            && (
                                (st != null && st.userName.Trim().ToLower() == User.Identity.Name.Trim().ToLower())
                                || (isAdmin == true)
                              )
                            && (branchID == null || c.branchID == branchID)
                   orderby ls.repaymentDate descending
                   select new
                   {
                       clientID = c.accountNumber,
                       clientName = c.surName + ", " + c.otherNames,
                       amountDue = ls.principalBalance + ls.interestBalance,
                       dateDue = ls.repaymentDate,
                       c.categoryID,
                       l.loanID,
                       staffName = ((st == null) ? "" : st.surName + ", " + st.otherNames),
                       loanNo = l.loanNo,
                   }).ToList();
        gridDue.DataSource = due; 
    }

    protected void gridApp_Load(object sender, EventArgs e)
    {
        int? branchID = null;
        if (Session["branchID"] != null && Session["branchID"] != "")
        {
            branchID = int.Parse(Session["branchID"].ToString());
        }
        var app = (
                                from l in le.loans
                                join c in le.clients on l.clientID equals c.clientID
                                join s in le.staffs on l.staffID equals s.staffID into stf
                                from st in stf.DefaultIfEmpty()
                                where l.loanStatusID <= 2
                                && (branchID == null || c.branchID == branchID)
                                select new
                                {
                                    clientID = c.accountNumber,
                                    clientName = c.surName + ", " + c.otherNames,
                                    l.amountRequested,
                                    l.applicationDate,
                                    c.categoryID,
                                    l.loanID,
                                    staffName = ((st == null) ? "" : st.surName + ", " + st.otherNames),
                                    loanNo = l.loanNo,
                                }).ToList();
        gridApp.DataSource = app;
    }

    protected void gridUnd_Load(object sender, EventArgs e)
    {
        int? branchID = null;
        if (Session["branchID"] != null && Session["branchID"] != "")
        {
            branchID = int.Parse(Session["branchID"].ToString());
        }
        var und = (
                        from l in le.loans
                        join c in le.clients on l.clientID equals c.clientID
                        join s in le.staffs on l.staffID equals s.staffID into stf
                        from st in stf.DefaultIfEmpty()
                        where l.loanStatusID == 3
                                && (branchID == null || c.branchID == branchID)
                        select new
                        {
                            clientID = c.accountNumber,
                            clientName = c.surName + ", " + c.otherNames,
                            l.amountRequested,
                            l.applicationDate,
                            l.amountApproved,
                            l.finalApprovalDate,
                            c.categoryID,
                            l.loanID,
                            staffName = ((st == null) ? "" : st.surName + ", " + st.otherNames),
                            loanNo = l.loanNo,
                        }).ToList();
        gridUnd.DataSource = und;
    }

    protected void cboBranch_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        Session["branchID"] = cboBranch.SelectedValue;
        Bind();
    }

}
