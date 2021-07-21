using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace coreERP.hc.payroll
{
    public partial class postingAccounts : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le = new coreLogic.coreLoansEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var acc = le.payrollPostingAccounts.FirstOrDefault();
                if (acc != null)
                {
                    try
                    {
                        cboDed.SelectedValue = acc.voluntaryDeductionsAccountID.ToString();
                        cboExpense.SelectedValue = acc.payrollExpenseAccountID.ToString();
                        cboLoanDed.SelectedValue = acc.loansRepaymentsAccountID.ToString();
                        cboLoans.SelectedValue = acc.loansReceivableAccountID.ToString();
                        cboNet.SelectedValue = acc.netSalaryAccountID.ToString();
                        cboPension.SelectedValue = acc.pensionsPayableAccountID.ToString();
                        cboTax.SelectedValue = acc.taxPayableAccountID.ToString();
                    }
                    catch (Exception) { }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if(cboDed.SelectedValue  != "" &&
                    cboExpense.SelectedValue != "" &&
                    cboLoanDed.SelectedValue != "" &&
                    cboLoans.SelectedValue  != "" &&
                    cboNet.SelectedValue  != "" &&
                    cboPension.SelectedValue != "" &&
                    cboTax.SelectedValue != "" &&
                    cboOvertime.SelectedValue!="")
            {
                var acc = le.payrollPostingAccounts.FirstOrDefault();
                if (acc == null)
                {
                    acc = new coreLogic.payrollPostingAccount();
                    le.payrollPostingAccounts.Add(acc);
                }
                acc.taxPayableAccountID = int.Parse(cboTax.SelectedValue);
                acc.pensionsPayableAccountID = int.Parse(cboPension.SelectedValue);
                acc.netSalaryAccountID = int.Parse(cboNet.SelectedValue);
                acc.payrollExpenseAccountID = int.Parse(cboExpense.SelectedValue);
                acc.loansRepaymentsAccountID = int.Parse(cboLoanDed.SelectedValue);
                acc.loansReceivableAccountID = int.Parse(cboLoans.SelectedValue);
                acc.voluntaryDeductionsAccountID = int.Parse(cboDed.SelectedValue);
                le.SaveChanges();
                HtmlHelper.MessageBox("Accounts Saved Successfully");
            }
        }
    }
}