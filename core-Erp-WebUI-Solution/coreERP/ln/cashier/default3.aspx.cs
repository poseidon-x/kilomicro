using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.cashier
{
    public partial class _default3 : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        string catID="1";

        protected void Page_Load(object sender, EventArgs e)
        {
            catID = Request.Params["catID"];
            if (catID == null) catID = "1";
            le = new coreLogic.coreLoansEntities();
            if(!IsPostBack)
            {
                
            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            List<coreLogic.loan> loans = new List<coreLogic.loan>();
            List<coreLogic.deposit> deposits = new List<coreLogic.deposit>();
            List<coreLogic.investment> investments = new List<coreLogic.investment>();
            List<coreLogic.saving> savings = new List<coreLogic.saving>();
            List<coreLogic.client> clients = null;
            List<coreLogic.susuAccount> susuAccs = new List<coreLogic.susuAccount>();
            List<coreLogic.regularSusuAccount> regsusuAccs = new List<coreLogic.regularSusuAccount>();

            if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccNo.Text.Trim())).ToList();
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
            foreach (var cl in clients)
            { 
                foreach (var l in cl.loans
                    .Where(p => (p.loanStatusID==4) && (p.closed==false))
                    .OrderByDescending(p=>p.applicationDate))
                { 
                    if (l.balance > 0 || l.repaymentSchedules.Sum(p=>p.interestBalance)>0
                        || l.loanPenalties.Sum(p => p.penaltyBalance) > 0 || l.processingFeeBalance > 0)
                    {
                        loans.Add(l);
                    }
                } 
                foreach (var dp in cl.deposits.OrderByDescending(p => p.firstDepositDate))
                {
                    deposits.Add(dp);
                } 
                foreach (var dp in cl.savings.OrderByDescending(p => p.firstSavingDate))
                {
                    savings.Add(dp);
                } 
                foreach (var dp in cl.investments.OrderByDescending(p => p.firstinvestmentDate))
                {
                    investments.Add(dp);
                } 
                foreach (var su in cl.susuAccounts)
                {
                    susuAccs.Add(su);
                }

                foreach (var sa in cl.regularSusuAccounts)
                {
                    var contrib = 0.0;
                    var disb = 0.0;
                    var comm = 0.0;
                    if (sa.regularSusuContributions.Count > 0)
                    {
                        contrib = sa.regularSusuContributions.Sum(p => p.amount);
                        comm = Math.Ceiling(sa.regularSusuContributions.Count / 31.0) * sa.contributionRate - sa.commissionPaid;
                    }
                    if (sa.regularSusuWithdrawals.Count > 0)
                    {
                        disb = sa.regularSusuWithdrawals.Sum(p => p.amount);
                    }
                    sa.amountEntitled = contrib;
                    sa.netAmountEntitled = contrib - disb - comm;
                    sa.regularSusCommissionAmount = comm;
                    regsusuAccs.Add(sa);
                }
            }
            if (loans.Count > 0)
            {
                grid.DataSource = loans;
                grid.DataBind();
                grid.Visible = true;
                trLoan.Visible = true;
            }
            else
            {
                grid.Visible = false;
                trLoan.Visible = false;
            }

            if (deposits.Count > 0)
            {
                grid2.DataSource = deposits;
                grid2.DataBind();
                grid2.Visible = true;
                trDeposit.Visible = true;
            }
            else
            {
                grid2.Visible = false;
                trDeposit.Visible = false;
            }

            if (investments.Count > 0)
            {
                grid3.DataSource = investments;
                grid3.DataBind();
                grid3.Visible = true;
                trInvestment.Visible = true;
            }
            else
            {
                grid3.Visible = false;
                trInvestment.Visible = false;
            }

            if (savings.Count > 0)
            {
                grid4.DataSource = savings;
                grid4.DataBind();
                grid4.Visible = true;
                trSavings.Visible = true;
            }
            else
            {
                grid4.Visible = false;
                trSavings.Visible = false;
            }
            if (susuAccs.Count > 0)
            {
                grid5.DataSource = susuAccs;
                grid5.DataBind();
                grid5.Visible = true;
                trSusuAccount.Visible = true;
            }
            else
            {
                grid5.Visible = false;
                trSusuAccount.Visible = false;
            }
            if (regsusuAccs.Count > 0)
            {
                grid6.DataSource = regsusuAccs;
                grid6.DataBind();
                grid6.Visible = true;
                trRegularSusu.Visible = true;
            }
            else
            {
                grid6.Visible = false;
                trRegularSusu.Visible = false;
            }
        }

        protected void grid_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
             
        }

        protected void cboClient_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            List<coreLogic.loan> loans = new List<coreLogic.loan>();
            List<coreLogic.deposit> deposits = new List<coreLogic.deposit>();
            List<coreLogic.investment> investments = new List<coreLogic.investment>();
            List<coreLogic.saving> savings = new List<coreLogic.saving>();
            List<coreLogic.susuAccount> susuAccs = new List<coreLogic.susuAccount>();
            List<coreLogic.regularSusuAccount> regsusuAccs = new List<coreLogic.regularSusuAccount>();
            if (cboClient.SelectedValue != "")
            {
                int id = int.Parse(cboClient.SelectedValue);
                var cl = le.clients
                    .Include(p => p.savings)
                    .Include(p => p.savings.Select(q => q.savingType))
                    .FirstOrDefault(p => p.clientID == id);
                if (cl != null)
                {
                    foreach (var l in cl.loans
                        .Where(p => (p.loanStatusID == 3 || p.loanStatusID == 4) && (p.closed == false))
                        .OrderByDescending(p => p.applicationDate))
                    { 
                        if (l.balance > 0 || l.repaymentSchedules.Sum(p => p.interestBalance) > 0
                            || l.loanPenalties.Sum(p => p.penaltyBalance) > 0 || l.processingFeeBalance>0)
                        {
                            loans.Add(l);
                        }
                    } 
                    foreach (var dp in cl.deposits.OrderByDescending(p=>p.firstDepositDate))
                    {
                        deposits.Add(dp);
                    } 
                    foreach (var dp in cl.investments.OrderByDescending(p => p.firstinvestmentDate))
                    { 
                        investments.Add(dp);
                    } 
                    foreach (var dp in cl.savings
                        .OrderByDescending(p => p.firstSavingDate))
                    {
                        var staff = le.staffSavings.FirstOrDefault(p => p.savingID == dp.savingID);
                        if (staff != null && User.IsInRole("admin") == false)
                        {
                            dp.availableInterestBalance = 0.0;
                            dp.availablePrincipalBalance = 0.0;
                            dp.principalBalance = 0.0;
                            dp.interestBalance = 0.0;
                            dp.amountInvested = 0.0;
                            
                        }
                        savings.Add(dp);
                    } 
                    foreach (var su in cl.susuAccounts)
                    {
                        susuAccs.Add(su);
                    } 
                    foreach (var sa in cl.regularSusuAccounts)
                    {
                        var contrib = 0.0;
                        var disb = 0.0;
                        var comm = 0.0;
                        if (sa.regularSusuContributions.Count > 0)
                        {
                            contrib = sa.regularSusuContributions.Sum(p => p.amount);
                            comm = Math.Ceiling(sa.regularSusuContributions.Count / 31.0) * sa.contributionRate - sa.commissionPaid;
                        }
                        if (sa.regularSusuWithdrawals.Count > 0)
                        {
                            disb = sa.regularSusuWithdrawals.Sum(p => p.amount);
                        }
                        sa.amountEntitled = contrib;
                        sa.netAmountEntitled = contrib - disb - comm;
                        sa.regularSusCommissionAmount = comm;
                        regsusuAccs.Add(sa);
                    }
                }
            }
            if (loans.Count > 0)
            {
                grid.DataSource = loans;
                grid.DataBind();
                grid.Visible = true;
                trLoan.Visible = true;
            }
            else
            {
                grid.Visible = false;
                trLoan.Visible = false;
            }

            if (deposits.Count > 0)
            {
                grid2.DataSource = deposits;
                grid2.DataBind();
                grid2.Visible = true;
                trDeposit.Visible = true;
            }
            else
            {
                grid2.Visible = false;
                trDeposit.Visible = false;
            }

            if (investments.Count > 0)
            {
                grid3.DataSource = investments;
                grid3.DataBind();
                grid3.Visible = true;
                trInvestment.Visible = true;
            }
            else
            {
                grid3.Visible = false;
                trInvestment.Visible = false;
            }

            if (savings.Count > 0)
            {
                grid4.DataSource = savings;
                grid4.DataBind();
                grid4.Visible = true;
                trSavings.Visible = true;
            }
            else
            {
                grid4.Visible = false;
                trSavings.Visible = false;
            }
            if (susuAccs.Count > 0)
            {
                grid5.DataSource = susuAccs;
                grid5.DataBind();
                grid5.Visible = true;
                trSusuAccount.Visible = true;
            }
            else
            {
                grid5.Visible = false;
                trSusuAccount.Visible = false;
            }
            if (regsusuAccs.Count > 0)
            {
                grid6.DataSource = regsusuAccs;
                grid6.DataBind();
                grid6.Visible = true;
                trRegularSusu.Visible = true;
            }
            else
            {
                grid6.Visible = false;
                trRegularSusu.Visible = false;
            }
        }

        protected void cboClient_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            if (e.Text.Trim().Length > 2)
            {
                cboClient.Items.Add(new RadComboBoxItem("", ""));
                foreach (var cl in le.clients.Where(p =>  (p.surName.Contains(e.Text) || p.otherNames.Contains(e.Text) || p.companyName.Contains(e.Text)
                    || p.accountName.Contains(e.Text))).OrderBy(p => p.surName))
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                        ", " + cl.otherNames) + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }
            }
        }

        protected void grid_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                var loanID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["loanID"];
                var loan = le.loans.FirstOrDefault(p => p.loanID == loanID);
                if (loan != null)
                {
                    var countOfUndisbursed = le.loans.Where(p => p.clientID == loan.clientID && p.amountDisbursed < p.amountApproved).Count();
                    var countOfUnapproved = le.loans.Where(p => p.clientID == loan.clientID && p.amountApproved < 0).Count();
                    if (countOfUnapproved==0)
                    {
                        e.Item.OwnerTableView.Columns[0].Visible = false;
                    }
                    if (countOfUndisbursed == 0)
                    {
                        e.Item.OwnerTableView.Columns[1].Visible = false;
                    }
                }
            }
            catch (Exception) { }
        }

        protected void grid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            var item = e.Item as GridDataItem;
            if (item != null)
            {
                var id = int.Parse(item.GetDataKeyValue("loanID").ToString());
                var sa = le.loans.FirstOrDefault(p => p.loanID == id);
                if (sa != null)
                {
                    item.Cells[1].Text = (sa.amountDisbursed>=sa.amountApproved || sa.finalApprovalDate == null) ? "" : item.Cells[1].Text;
                    item.Cells[2].Text = (sa.balance<=3) ? "" : item.Cells[2].Text; 
                } 
            }
        }

        protected void grid2_ItemDataBound(object sender, GridItemEventArgs e)
        {
            var item = e.Item as GridDataItem;
            if (item != null)
            {
                var id = int.Parse(item.GetDataKeyValue("depositID").ToString());
                var sa = le.deposits.FirstOrDefault(p => p.depositID == id);
                if (sa != null)
                {
                    item.Cells[2].Text = (sa.principalBalance<1 && sa.interestBalance<1) ? "" : item.Cells[2].Text;
                } 
            }
        }

        protected void grid4_ItemDataBound(object sender, GridItemEventArgs e)
        {
            var item = e.Item as GridDataItem;
            if (item != null)
            {
                var id = int.Parse(item.GetDataKeyValue("savingID").ToString());
                var sa = le.savings.FirstOrDefault(p => p.savingID == id);
                if (sa != null)
                {
                    item.Cells[2].Text = (sa.principalBalance < 1 && sa.interestBalance < 1) ? "" : item.Cells[2].Text;
                }
                //item.Cells[0]
            }
        }

        protected void grid3_ItemDataBound(object sender, GridItemEventArgs e)
        {
            var item = e.Item as GridDataItem;
            if (item != null)
            {
                var id = int.Parse(item.GetDataKeyValue("investmentID").ToString());
                var sa = le.investments.FirstOrDefault(p => p.investmentID == id);
                if (sa != null)
                {
                    item.Cells[2].Text = (sa.principalBalance < 1 && sa.interestBalance < 1) ? "" : item.Cells[2].Text;
                } 
            }
        }

        protected void grid5_ItemDataBound(object sender, GridItemEventArgs e)
        {
            var item = e.Item as GridDataItem;
            if (item != null)
            {
                var id = int.Parse(item.GetDataKeyValue("susuAccountID").ToString());
                var sa = le.susuAccounts.FirstOrDefault(p => p.susuAccountID == id);
                if (sa != null)
                {
                    item.Cells[1].Text = (sa.disbursedBy != null && sa.disbursedBy != "") ? "" : item.Cells[1].Text;
                } 
            }
        }

        protected void grid6_ItemDataBound(object sender, GridItemEventArgs e)
        {
            var item = e.Item as GridDataItem;
            if (item != null)
            {
                var id = int.Parse(item.GetDataKeyValue("regularSusuAccountID").ToString());
                var sa = le.regularSusuAccounts.FirstOrDefault(p => p.regularSusuAccountID == id);
                if (sa != null)
                {
                    item.Cells[1].Text = (sa.disbursedBy != null && sa.disbursedBy != "") ? "" : item.Cells[1].Text;
                } 
            }
        }

    }
}