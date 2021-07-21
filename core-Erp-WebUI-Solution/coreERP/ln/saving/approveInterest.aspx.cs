using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.saving
{
    public partial class approveInterest : System.Web.UI.Page
    {
        IJournalExtensions journalextensions = new JournalExtensions();
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;
        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            ent = new coreLogic.core_dbEntities();
            if (!Page.IsPostBack)
            {
                cboClient.Items.Add(new RadComboBoxItem("", ""));
                foreach (var cl in le.clients.Where(p => p.clientTypeID == 1 || p.clientTypeID==2).OrderBy(p => p.surName))
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                    ", " + cl.otherNames) + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }

                cboCur.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in ent.currencies)
                {
                    cboCur.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.major_name,
                        r.currency_id.ToString()));
                }
                txtFxRate.Value = 1;

            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            var pro = ent.comp_prof.FirstOrDefault();
            foreach (RepeaterItem item in rpPenalty.Items)
            {
                var lblID = item.FindControl("lblID") as Label;
                var txtProposedAmount = item.FindControl("txtProposedAmount") as Telerik.Web.UI.RadNumericTextBox;
                var dtInterestDate = item.FindControl("dtInterestDate") as Telerik.Web.UI.RadDatePicker;
                var chkSelected = item.FindControl("chkSelected") as CheckBox;
                if (lblID != null && txtProposedAmount != null && chkSelected != null && chkSelected.Checked == true
                    && dtInterestDate != null)
                {
                    int id = int.Parse(lblID.Text);
                    var pen = le.savingSchedules.FirstOrDefault(p => p.savingScheduleID == id);
                    if (pen != null)
                    {
                        pen.interestPayment = txtProposedAmount.Value.Value;
                        pen.expensed = true;
                        pen.temp = false;
                        //pen.savingReference.Load();
                        pen.saving.interestAccumulated += txtProposedAmount.Value.Value;
                        pen.saving.interestBalance += txtProposedAmount.Value.Value;
                        //pen.saving.savingTypeReference.Load();
                        //pen.saving.clientReference.Load();
                        //pen.saving.savingInterests.Load();
                        //pen.saving.savingAdditionals.Load();
                        pen.repaymentDate = dtInterestDate.SelectedDate.Value;
                        pen.saving.lastInterestDate = dtInterestDate.SelectedDate.Value;

                        var inte = new coreLogic.savingInterest
                        {
                            interestAmount = txtProposedAmount.Value.Value,
                            fromDate = pen.repaymentDate,
                            toDate = pen.repaymentDate,
                            creation_date = DateTime.Now,
                            creator = User.Identity.Name,
                            fxRate = txtFxRate.Value.Value,
                            interestDate = pen.repaymentDate,
                            principal = pen.saving.principalBalance,
                            proposedAmount = txtProposedAmount.Value.Value,
                            localAmount = txtProposedAmount.Value.Value * txtFxRate.Value.Value,
                            savingID = pen.saving.savingID,
                            interestBalance=txtProposedAmount.Value.Value
                        };
                        pen.saving.savingInterests.Add(inte);
                        var jb = journalextensions.Post("LN", pen.saving.savingType.interestExpenseAccountID.Value,
                            pen.saving.savingType.interestPayableAccountID, (txtProposedAmount.Value.Value
                            * txtFxRate.Value.Value),
                            "Interest Calculated on Savings - " + pen.saving.client.surName + "," + pen.saving.client.otherNames,
                            pro.currency_id.Value, pen.repaymentDate, pen.saving.savingNo, ent, User.Identity.Name, pen.saving.client.branchID);
                        ent.jnl_batch.Add(jb);
                        var ln = pen.saving;
                        var cur = ent.currencies.FirstOrDefault(p => p.currency_id == ln.currencyID);
                        var diff = txtFxRate.Value.Value - ln.fxRate;
                        if (ln.currencyID != pro.currency_id)
                        {
                            foreach (var sa in ln.savingAdditionals)
                            {
                                if (sa.lastPrincipalFxGainLoss != 0)
                                {
                                    if (sa.lastPrincipalFxGainLoss > 0)
                                    {
                                        jb = journalextensions.Post("LN", ln.savingType.accountsPayableAccountID.Value,
                                            ln.savingType.fxUnrealizedGainLossAccountID, sa.lastPrincipalFxGainLoss,
                                            "RVSL: Savings - Fx Loss - "
                                            + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                                            pro.currency_id.Value, pen.repaymentDate, ln.savingNo, ent, "SYSTEM", pen.saving.client.branchID);
                                        ent.jnl_batch.Add(jb);
                                    }
                                    else
                                    {
                                        jb = journalextensions.Post("LN", ln.savingType.fxUnrealizedGainLossAccountID,
                                            ln.savingType.accountsPayableAccountID.Value, -sa.lastPrincipalFxGainLoss,
                                            "RVSL: Savings - Fx Gain - "
                                            + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                                            pro.currency_id.Value, pen.repaymentDate, ln.savingNo, ent, "SYSTEM", pen.saving.client.branchID);
                                        ent.jnl_batch.Add(jb);
                                    }
                                }

                                if (sa.fxRate < txtFxRate.Value.Value && sa.principalBalance > 0)
                                {
                                    jb = journalextensions.Post("LN", ln.savingType.fxUnrealizedGainLossAccountID,
                                        ln.savingType.accountsPayableAccountID.Value,
                                        sa.principalBalance * diff,
                                        "Savings - Fx Loss - "
                                        + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                                        pro.currency_id.Value, pen.repaymentDate, ln.savingNo, ent, "SYSTEM", pen.saving.client.branchID);
                                    ent.jnl_batch.Add(jb);
                                }
                                else if (sa.fxRate > txtFxRate.Value.Value && sa.principalBalance > 0)
                                {
                                    jb = journalextensions.Post("LN", ln.savingType.accountsPayableAccountID.Value,
                                        ln.savingType.fxUnrealizedGainLossAccountID,
                                        -(ln.principalBalance * diff),
                                        "Savings - Fx Gain - "
                                        + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                                        pro.currency_id.Value, pen.repaymentDate, ln.savingNo, ent, "SYSTEM", pen.saving.client.branchID);
                                    ent.jnl_batch.Add(jb);
                                }
                                sa.localAmount = sa.principalBalance * txtFxRate.Value.Value;
                                sa.lastPrincipalFxGainLoss = sa.principalBalance * diff;
                            }
                            foreach (var si in ln.savingInterests)
                            {
                                if (si.savingInterestID != inte.savingInterestID)
                                {
                                    if (si.lastInterestFxGainLoss != 0)
                                    {
                                        if (si.lastInterestFxGainLoss > 0)
                                        {
                                            jb = journalextensions.Post("LN", ln.savingType.interestPayableAccountID,
                                                ln.savingType.fxUnrealizedGainLossAccountID, si.lastInterestFxGainLoss,
                                                "RVSL: Savings - Fx Loss - "
                                                + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                                                pro.currency_id.Value, pen.repaymentDate, ln.savingNo, ent, "SYSTEM", pen.saving.client.branchID);
                                            ent.jnl_batch.Add(jb);
                                        }
                                        else
                                        {
                                            jb = journalextensions.Post("LN", ln.savingType.fxUnrealizedGainLossAccountID,
                                                ln.savingType.interestPayableAccountID, -si.lastInterestFxGainLoss,
                                                "RVSL: Savings - Fx Gain - "
                                                + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                                                pro.currency_id.Value, pen.repaymentDate, ln.savingNo, ent, "SYSTEM", pen.saving.client.branchID);
                                            ent.jnl_batch.Add(jb);
                                        }
                                    }
                                    if (si.fxRate < txtFxRate.Value.Value && si.interestBalance > 0)
                                    {
                                        jb = journalextensions.Post("LN", ln.savingType.fxUnrealizedGainLossAccountID,
                                            ln.savingType.interestPayableAccountID,
                                            (si.interestBalance * diff),
                                            "Savings - Fx Loss - "
                                            + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                                            pro.currency_id.Value, pen.repaymentDate, ln.savingNo, ent, "SYSTEM", pen.saving.client.branchID);
                                        ent.jnl_batch.Add(jb);
                                    }
                                    else if (si.fxRate > txtFxRate.Value.Value && si.interestBalance > 0)
                                    {
                                        jb = journalextensions.Post("LN", ln.savingType.interestPayableAccountID,
                                            ln.savingType.fxUnrealizedGainLossAccountID,
                                            -(si.interestBalance * diff),
                                            "Savings - Fx Gain - "
                                            + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                                            pro.currency_id.Value, pen.repaymentDate, ln.savingNo, ent, "SYSTEM", pen.saving.client.branchID);
                                        ent.jnl_batch.Add(jb);
                                    }
                                    si.localAmount = si.interestBalance * txtFxRate.Value.Value;
                                    si.lastInterestFxGainLoss = si.interestBalance * diff;
                                }
                            }
                            ln.localAmount = ln.principalBalance * txtFxRate.Value.Value;
                            ln.lastPrincipalFxGainLoss = ln.principalBalance * diff;
                            ln.lastInterestFxGainLoss = ln.interestBalance * diff;
                        }
                    }
                }
            }
            le.SaveChanges();
            ent.SaveChanges();
            HtmlHelper.MessageBox2("Interest on Savings Data Saved Successfully!", ResolveUrl("~/ln/saving/approveInterest.aspx"), "coreERP©: Successful", IconType.ok);  
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void cboClient_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            OnChange();
        }

        public void CalculateInterest(int clientID, DateTime date)
        {
            try
            {
                var lns2 = le.savingSchedules.Where(p => p.expensed==false && p.saving.client.clientID==clientID).ToList();
                foreach (var ln in lns2)
                {
                    le.savingSchedules.Remove(ln);
                }
                var lns = le.savingSchedules.Where(p => p.saving.principalBalance > 0 && (p.saving.lastInterestDate == null || p.saving.lastInterestDate < date)
                    && p.saving.client.clientID == clientID).ToList();
                foreach (var ln in lns)
                {
                    ////ln.savingReference.Load();
                    //ln.saving.clientReference.Load(); 

                    //ln.saving.savingTypeReference.Load();                    
                }
                le.SaveChanges();
            }
            catch (Exception x)
            {
            }
        }

        protected void dtInterestDate1_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            OnChange();
        }

        private void OnChange()
        {
            if (cboClient.SelectedValue != "" && dtInterestDate1.SelectedDate != null)
            {
                var id = -1;
                if (cboClient.SelectedValue != "")
                {
                    id = int.Parse(cboClient.SelectedValue);
                    CalculateInterest2(id, dtInterestDate1.SelectedDate.Value);
                }

                var lns = le.savingSchedules.Where(p=> p.expensed==false && p.saving.principalBalance>0
                    && p.saving.client.clientID == id && p.repaymentDate<=dtInterestDate1.SelectedDate.Value).ToList();
                foreach (var ln in lns)
                {
                    //ln.savingReference.Load();
                    //ln.saving.clientReference.Load();

                    //ln.saving.savingTypeReference.Load();
                }

                rpPenalty.DataSource = lns;
                rpPenalty.DataBind();
            }
        }

        public void CalculateInterest2(int clientID, DateTime date)
        {
            try
            { 
                var dps = le.savings.Where(p => p.client.clientID == clientID && p.principalBalance>0).ToList();

                var lns = le.savingSchedules.Where(p => p.saving.principalBalance > 0 && p.expensed == false
                    && p.saving.client.clientID == clientID).ToList();


                foreach (var dp in dps)
                {
                    
                    List<DateTime> listInt = new List<DateTime>();
                    List<DateTime> listPrinc = new List<DateTime>();
                    List<DateTime> listAll = new List<DateTime>();

                    DateTime date2 = (dp.lastInterestDate == null) ? dp.firstSavingDate : dp.lastInterestDate.Value;
                    //dp.savingSchedules.Load();
                    DateTime lastDate=date2;
                    foreach (var sched in dp.savingSchedules.ToList())
                    {
                        if ((sched.repaymentDate >= date2 && sched.repaymentDate<=date)&&(sched.authorized==false)&&(sched.principalPayment==0 && sched.interestPayment==0))
                        {
                            le.savingSchedules.Remove(sched);
                        }
                        else if ((sched.repaymentDate >= date2 && sched.repaymentDate<=date) && (sched.principalPayment > 0)&& (sched.expensed==false))
                        {
                            sched.interestPayment = 0;
                        }
                        else if ((sched.repaymentDate >= date2 && sched.repaymentDate<=date)&&(sched.authorized==false)&&(sched.expensed==false))
                        {
                            le.savingSchedules.Remove(sched);
                        }
                    }
                    var totalPrinc = dp.amountInvested - dp.savingSchedules.Sum(p => p.principalPayment);
                    int i = 1;
                    var totalInt = dp.principalBalance * (date - date2).TotalDays / 30.0 * (dp.interestRate) / 100.0;
                    var intererst = totalInt;
                    var princ = 0.0;

                    if (dp.savingSchedules.FirstOrDefault(p => p.repaymentDate == date) != null)
                    {
                        dp.savingSchedules.FirstOrDefault(p => p.repaymentDate == date).interestPayment += intererst;
                    }
                    else
                    {
                        dp.savingSchedules.Add(new coreLogic.savingSchedule
                        {
                            interestPayment = totalInt,
                            principalPayment = 0,
                            repaymentDate = date,
                            authorized = false,
                            temp = true
                        });
                    }
                }

                le.SaveChanges();
            }
            catch (Exception x)
            {
            }
        }

        protected void cboCur_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboCur.SelectedValue != "")
            {
                int id = int.Parse(cboCur.SelectedValue);
                var cur = ent.currencies.FirstOrDefault(p => p.currency_id == id);
                if (cur != null)
                {
                    txtFxRate.Value = cur.current_buy_rate;
                }
            }
        }

    }
}