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

public partial class PendingAapprovals : System.Web.UI.Page 
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

    private void Bind()
    {
        int? branchID = null;
        if (Session["branchID"] != null && Session["branchID"] != "")
        {
            branchID = int.Parse(Session["branchID"].ToString());
        } 
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
