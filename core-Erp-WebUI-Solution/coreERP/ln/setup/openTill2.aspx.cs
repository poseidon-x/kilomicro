using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace coreERP.ln.setup
{
    public partial class openCloseTill2 : System.Web.UI.Page
    {
        IRepaymentsManager rpmtMgr = new RepaymentsManager();
        IDisbursementsManager disbMgr = new DisbursementsManager();
       
        coreLogic.coreSecurityEntities sec = new coreLogic.coreSecurityEntities();
        coreLogic.coreLoansEntities le = new coreLogic.coreLoansEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                dtStartDate.SelectedDate = DateTime.Today;
                dtpEndDate.SelectedDate = DateTime.Today;

                cboUserName.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in sec.users.Where(r=>r.is_active).OrderBy(p => p.full_name))
                {
                    if (le.cashiersTills.FirstOrDefault(p => p.userName.ToLower() == r.user_name.ToLower()) != null)
                    {
                        cboUserName.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.full_name + " (" + r.user_name + ")", r.user_name));
                    }
                    cboUserName.SelectedValue = User.Identity.Name.Trim();
                }
            }
        }

        protected void btnOpen_Click(object sender, EventArgs e)
        {
            if (cboUserName.SelectedValue != "" && dtStartDate.SelectedDate != null )
            {
                var u = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower() == cboUserName.SelectedValue.ToLower());
                if (u != null)
                {
                    var date = dtStartDate.SelectedDate.Value;
                    var changed = false;
                    if (date > DateTime.Now)
                    {
                        HtmlHelper.MessageBox("A future till date cannot be openned.", "coreERP©: Failed", IconType.deny);
                        return;
                    }
                    while (date <= dtpEndDate.SelectedDate.Value)
                    {
                        var day = le.cashiersTillDays.FirstOrDefault(p => p.tillDay == date && p.cashiersTillID == u.cashiersTillID);
                        if (day != null && day.open == true)
                        {
                            HtmlHelper.MessageBox("Selected User Already Open for Selected Day", "coreERP©: Failed", IconType.deny);
                            return;
                        }
                        else if (day != null)
                        {
                            day.open = true; 
                            date = date.AddDays(1);
                            changed = true;
                        }
                        else
                        {
                            day = new coreLogic.cashiersTillDay
                            {
                                cashiersTillID = u.cashiersTillID,
                                creation_date = DateTime.Now,
                                creator = User.Identity.Name,
                                open = true,
                                tillDay = date
                            };
                            le.cashiersTillDays.Add(day);
                            date = date.AddDays(1);
                            changed = true;
                        }
                    }
                    if (changed == true)
                    {
                        le.SaveChanges();
                        HtmlHelper.MessageBox("Till Openned Succesfully for " + cboUserName.Text, "coreERP©: Successful", IconType.ok);
                    }
                }
                else
                {
                    HtmlHelper.MessageBox("No Till Defined for Selected User");
                }
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            IInvestmentManager ivMgr = new InvestmentManager();
            coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
            coreLogic.jnl_batch batch = null;
            if (cboUserName.SelectedValue != "" && dtStartDate.SelectedDate != null && dtpEndDate.SelectedDate!= null 
                && dtpEndDate.SelectedDate>=dtStartDate.SelectedDate)
            {
                var u = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower() == cboUserName.SelectedValue.ToLower());
                if (u != null)
                {
                    var date = dtStartDate.SelectedDate.Value;
                    var changed = false;
                    while (date <= dtpEndDate.SelectedDate.Value)
                    {
                        var day = le.cashiersTillDays.FirstOrDefault(p => p.tillDay == date && p.cashiersTillID == u.cashiersTillID);
                        if (day != null && day.open == false)
                        {
                            HtmlHelper.MessageBox("Selected User Already Closed for Selected Day");
                            return;
                        }
                        else if (day != null)
                        {
                            var rcpt = le.cashierReceipts.Where(p => p.txDate == date && p.cashiersTill.userName.ToLower()
                                == cboUserName.SelectedValue.ToLower() && p.posted == true && p.closed == false && p.paymentModeID == 1);
                            foreach (var r in rcpt)
                            {
                                r.closed = true;
                                rpmtMgr.CloseCashierReceipt(le, r.loan, r, ent, r.cashiersTill.userName, ref batch);
                            }

                            var das = le.savingAdditionals.Where(p => p.savingDate == date && p.creator.ToLower()
                                == cboUserName.SelectedValue.ToLower() && p.posted == true && p.closed == false && p.modeOfPaymentID == 1);
                            foreach (var r in das)
                            {
                                ivMgr.CloseSavingAdditional(r, r.creator, ent, le, u, ref batch);
                                r.closed = true;
                            }

                            var dws = le.savingWithdrawals.Where(p => p.withdrawalDate == date && p.creator.ToLower()
                                == cboUserName.SelectedValue.ToLower() && p.posted == true && p.closed == false && p.modeOfPaymentID == 1);
                            foreach (var r in dws)
                            {
                                ivMgr.CloseSavingsWithdrawal(r, r.creator, ent, le, u, ref batch);
                                r.closed = true;
                            }

                            day.open = false;
                            if (batch != null) ent.jnl_batch.Add(batch);
                            changed = true;
                        }
                        date = date.AddDays(1);
                    }
                    if (changed == true)
                    {
                        le.SaveChanges();
                        ent.SaveChanges();
                        HtmlHelper.MessageBox("Till Closed Succesfully for " + cboUserName.Text);
                    }
                    else
                    {
                        HtmlHelper.MessageBox("No Till Defined for Selected User");
                    }
                }
                else
                {
                    HtmlHelper.MessageBox("No Till Defined for Selected User");
                }
            }
        }
    }
}