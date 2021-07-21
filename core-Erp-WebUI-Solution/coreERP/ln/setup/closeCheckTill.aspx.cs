using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.setup
{
    public partial class closeCheckTill : System.Web.UI.Page
    {
        IRepaymentsManager rpmtMgr = new RepaymentsManager();
        coreLogic.coreSecurityEntities sec = new coreLogic.coreSecurityEntities();
        coreLogic.coreLoansEntities le = new coreLogic.coreLoansEntities();
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
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
            jnl_batch bat = null;
            List<int> loanIDs = new List<int>();
            Dictionary<int, jnl_batch> lsBat = new Dictionary<int, jnl_batch>();
            foreach (RepeaterItem item in rpPenalty.Items)
            {
                var lblID = item.FindControl("lblID") as Label;
                var chkSelected = item.FindControl("chkSelected") as CheckBox;
                var dtDate = item.FindControl("dtDate") as RadDatePicker;
                var cboBank = item.FindControl("cboBank") as RadComboBox;
                if (lblID != null && chkSelected != null && chkSelected.Checked == true  && dtDate != null && cboBank != null)
                {
                    int id = int.Parse(lblID.Text);
                    var r = le.cashierReceipts.FirstOrDefault(p => p.cashierReceiptID == id);
                    if (r != null)
                    {
                        if (cboBank.SelectedValue != "") r.bankID = int.Parse(cboBank.SelectedValue);
                        r.txDate = dtDate.SelectedDate.Value;
                        
                        int? crAccNo = null;

                        jnl_batch batch = null;
                        multiPaymentClient mpc = (
                                from m in le.multiPaymentClients
                                from r2 in le.cashierReceipts 
                                from r3 in le.cashierReceipts
                                where r2.checkNo==r3.checkNo && r2.clientID==r3.clientID
                                    && r2.loanID == r3.loanID
                                    && r2.cashierReceiptID==r3.cashierReceiptID
                                    && r3.cashierReceiptID==r.cashierReceiptID
                                    && m.cashierReceiptID==r2.cashierReceiptID
                                select m
                            ).FirstOrDefault();
                        rpmtMgr.CashierCheckReceipt(le, r.loan, r, ent, r.cashiersTill.userName, null, ref batch);
                        if (r.checkNo != null)
                        { 
                            var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == r.bankID).accts.acct_id;
                            var acc = ent.def_accts.FirstOrDefault(p => p.code == "RF");
                            if (acc != null)
                            {
                                var pro = ent.comp_prof.FirstOrDefault();
                                crAccNo = acc.accts.acct_id; 
                                var je = (new JournalExtensions());

                                var js = batch.jnl.Where(p => p.accts.acct_id == ba
                                    && p.description.Contains("Check paid by") == false).ToList();
                                foreach (var j2 in js)
                                {
                                    ent.Entry(j2).State = System.Data.Entity.EntityState.Detached;
                                }

                                if (mpc != null && loanIDs.Contains(r.loanID) == false && mpc.checkAmount>0)
                                {
                                    loanIDs.Add(r.loanID);
                                    var jb = je.Post("LN2", ba, crAccNo.Value,
                                        mpc.checkAmount, "Check paid by "
                                        + r.client.surName + ", " + r.client.otherNames, pro.currency_id.Value,
                                        r.txDate, r.loan.loanNo, ent, r.cashiersTill.userName, r.client.branchID);
                                    mpc.posted = true;
                                    var j = jb.jnl.FirstOrDefault(p => p.accts.acct_id == crAccNo);
                                    j.crdt_amt = mpc.balance;                                    
                                    foreach (var j2 in jb.jnl.ToList())
                                    {
                                        batch.jnl.Add(j2);
                                    }
                                }
                                if (lsBat[r.loanID] == null)
                                {
                                    bat = batch;
                                    lsBat.Add(r.loanID, bat);
                                }
                                else
                                {
                                    bat = lsBat[r.loanID];
                                    js = batch.jnl.ToList();
                                    foreach (var j2 in js)
                                    {
                                        bat.jnl.Add(j2);
                                    }
                                    ent.Entry(batch).State = System.Data.Entity.EntityState.Detached;
                                } 
                            }
                        }
                         
                        r.closed = true;
                    }
                }
            }
            le.SaveChanges();
            ent.SaveChanges();
            le.SaveChanges();
            OnChange();
            HtmlHelper.MessageBox("Check Till Closed Succesfully for " + cboUserName.Text,"coreERP©: Successful", IconType.ok);
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
            if (cboUserName.SelectedValue != "" && dtDate.SelectedDate != null)
            {
                var u = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower() == cboUserName.SelectedValue.ToLower());
                if (u != null)
                {
                    var day = le.cashiersTillDays.FirstOrDefault(p => p.tillDay == dtDate.SelectedDate.Value && p.cashiersTillID == u.cashiersTillID);
                    if (day != null)
                    {
                        coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                        coreLogic.jnl_batch batch = null;

                        var rcpt = le.cashierReceipts.Where(p => p.txDate == dtDate.SelectedDate.Value && p.cashiersTill.userName.ToLower()
                            == cboUserName.SelectedValue.ToLower() && p.posted == true && p.closed == false && p.paymentModeID == 2).ToList();
                        
                        rpPenalty.DataSource = rcpt;
                        rpPenalty.DataBind();
                    } 
                } 
            }
        }

        protected void rpPenalty_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var ent =new coreLogic.core_dbEntities();

            if (e.Item.ItemType == ListItemType.Item)
            {
                var cboBank = e.Item.FindControl("cboBank") as RadComboBox;
                if (cboBank != null)
                {
                    cboBank.Items.Add(new RadComboBoxItem("", ""));
                    foreach (var r in ent.bank_accts)
                    {
                        cboBank.Items.Add(new RadComboBoxItem(r.bank_acct_desc + " (" + r.bank_acct_num + ")",
                        r.bank_acct_id.ToString()));
                    }
                    var cr = e.Item.DataItem as coreLogic.cashierReceipt;
                    if (cr != null)
                    {
                        cboBank.SelectedValue = cr.bankID.ToString();
                    }
                }
            }
        }
    }
}