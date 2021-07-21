using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.cashier
{
    public partial class disbursePayroll : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;

        protected void Page_Load(object sender, EventArgs e)
        {
            le=new coreLogic.coreLoansEntities();
            ent = new coreLogic.core_dbEntities();
            if (!Page.IsPostBack)
            {
                dtDate.SelectedDate = DateTime.Now.Date;

                cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in ent.bank_accts)
                {
                    cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.bank_acct_desc + " (" + r.bank_acct_num + ")",
                        r.bank_acct_id.ToString()));
                }

                var lns = le.loans.Where(p => p.loanStatusID == 3 && p.client.categoryID==5).ToList();
                for (int i = lns.Count - 1; i >= 0; i--)
                {
                    var l = lns[i];
                    if (le.cashierDisbursements.Count(p => p.loanID == l.loanID) > 0)
                    {
                        lns.Remove(l);
                    }
                }
                foreach (var l in lns)
                { 
                    //l.clientReference.Load();
                }
                gridLoans.DataSource = lns;
                gridLoans.DataBind();
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            var ct = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.Trim().ToLower());
            if (ct == null)
            {
                HtmlHelper.MessageBox("There is no till defined for the currently logged in user (" + User.Identity.Name + ")");
                return;
            }
            var ctd = le.cashiersTillDays.FirstOrDefault(p => p.cashiersTillID == ct.cashiersTillID && p.tillDay == dtDate.SelectedDate.Value
                && p.open == true);
            if (ctd == null)
            {
                HtmlHelper.MessageBox("The till for the selected date has not been opened for this user (" + User.Identity.Name + ")");
                return;
            }
            int? bankID = null;
            if (cboBank.SelectedValue != null && cboBank.SelectedValue != "")
                bankID = int.Parse(cboBank.SelectedValue);
            foreach (GridItem item in gridLoans.Items)
            {
                if (item != null && item is GridDataItem)
                {
                    var loanID = (int)item.OwnerTableView.DataKeyValues[item.ItemIndex]["loanID"];
                    var txt = item.FindControl("txtAmount") as RadNumericTextBox;
                    var chk = item.FindControl("chkSelected") as CheckBox;
                    var txtCN = item.FindControl("txtCheckNo") as RadTextBox;
                    var ln = le.loans.FirstOrDefault(p => p.loanID == loanID);
                    if (txt != null && txt.Value != null && ln != null && chk != null && chk.Checked==true)
                    {
                        var user = (new coreLogic.coreSecurityEntities()).users.First(p => p.user_name.ToLower().Trim() == User.Identity.Name.ToLower().Trim());
                        if (user.accessLevel.disbursementLimit < txt.Value)
                        {
                            HtmlHelper.MessageBox("The amount to be disbursed is beyond your access level",
                                                        "coreERP©: Failed", IconType.deny);
                            return;
                        }
                        var soFar = ln.cashierDisbursements.Sum(p=>p.amount);
                        if (soFar == null) soFar = 0;

                        var checkNo = (txtCN.Text.Length > 0) ? txtCN.Text : txtCheckNo.Text;
                        if (ln.amountApproved < txt.Value.Value+soFar)
                        {
                            HtmlHelper.MessageBox("Amount to disburse is greater than approved amount!");
                            return;
                        }
                        var cd = new coreLogic.cashierDisbursement
                        {
                            amount = txt.Value.Value,
                            bankID = bankID,
                            checkNo = checkNo,
                            clientID = ln.client.clientID,
                            loanID = ln.loanID,
                            paymentModeID = int.Parse(cboPaymentMode.SelectedValue),
                            posted = false,
                            txDate = dtDate.SelectedDate.Value,
                            cashierTillID = ct.cashiersTillID,
                            postToSavingsAccount = false
                        };
                        le.cashierDisbursements.Add(cd);
                    }
                }
            }
            le.SaveChanges();

            Session["loanGuarantors"] = null;
            Session["loanCollaterals"] = null;
            Session["loan.cl"] = null;
            Session["loan"] = null;
            HtmlHelper.MessageBox2("The disbursement has been received successfully",
                ResolveUrl("~/ln/cashier/disbursePayroll.aspx"), "coreERP©: Successful", IconType.ok);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
         
    }
}