using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data.Entity.Core;
using System.Data.Entity;
using coreLogic;

namespace coreERP.ln.setup
{
    public partial class cashUpTill : System.Web.UI.Page
    {
        IJournalExtensions journalextensions = new JournalExtensions();
        coreLogic.coreSecurityEntities sec = new coreLogic.coreSecurityEntities();
        coreLogic.coreLoansEntities le = new coreLogic.coreLoansEntities();
        coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
        coreReports.reportEntities rent = new coreReports.reportEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cboUserName.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in sec.users.OrderBy(p => p.full_name))
                {
                    if (le.cashiersTills.FirstOrDefault(p => p.userName.ToLower() == r.user_name.ToLower()) != null)
                    {
                        cboUserName.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.full_name + " (" + r.user_name + ")", r.user_name));
                    }
                }
                cboAcc.Items.Add(new RadComboBoxItem("", ""));
                foreach (var r in ent.vw_accounts.Where(p => p.cat_code <= 3).OrderBy(p => p.acc_num))
                {
                    cboAcc.Items.Add(new RadComboBoxItem(r.acc_num + " | " + r.acc_name, r.acct_id.ToString()));
                }
            }
        }
         
        protected void btnOpen_Click(object sender, EventArgs e)
        {
            if (cboUserName.SelectedValue != "" && dtDate.SelectedDate != null && cboAcc.SelectedValue!="")
            {
                var u = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower() == cboUserName.SelectedValue.ToLower());
                if (u != null)
                {
                    var acctID = u.accountID;
                    var bankAcctID = int.Parse(cboAcc.SelectedValue);
                    var accBal = ent.get_acc_bal(acctID, dtDate.SelectedDate.Value, null).FirstOrDefault();
                    if (accBal !=null && accBal.Value != 0)
                    {
                        var prof = ent.comp_prof.FirstOrDefault();
                        if (accBal > 0)
                        {
                            journalextensions.Post("C/S", bankAcctID, acctID, accBal.Value,
                                "Cashing Up Cashier's Till '" + cboUserName.Text + "'",
                                prof.currency_id.Value, dtDate.SelectedDate.Value, cboUserName.SelectedValue, ent, User.Identity.Name,
                                null);
                        }
                        else
                        {
                            journalextensions.Post("C/S", acctID, bankAcctID, -accBal.Value,
                                "Cashing Up Cashier's Till '" + cboUserName.Text + "'",
                                prof.currency_id.Value, dtDate.SelectedDate.Value, cboUserName.SelectedValue, ent, User.Identity.Name,
                                null);
                        }
                        le.SaveChanges();
                        ent.SaveChanges();
                        HtmlHelper.MessageBox2("Till cashed up for selected user successfully!", ResolveUrl("~/ln/setup/postTill.aspx"), "coreERP©: Successful", IconType.ok);
                    }
                }
                else
                {
                    HtmlHelper.MessageBox("No Till Defined for Selected User", "coreERP©: Failed", IconType.deny);
                }
            }
        }
 
        protected void dtDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            OnChange();
        }

        protected void cboUserName_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            OnChange();
        }

        private void OnChange()
        {
            if (dtDate.SelectedDate != null && cboUserName.SelectedValue != "")
            {
                var till = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == cboUserName.SelectedValue.ToLower().Trim());
                if (till != null)
                {
                    var acctID = till.accountID;
                   var accBal = ent.get_acc_bal(acctID, dtDate.SelectedDate.Value, null).FirstOrDefault();
                    if (accBal !=null)
                    {                      
                        lblBalance.Text = accBal.Value.ToString("#,##0.#0;(#,##0.#0);0.00");
                        if (accBal.Value != 0)
                        {
                            btnOpen.Enabled = true;
                        }
                        return;
                    }
                }
            }

            btnOpen.Enabled = false;
        }
   
    }
}