using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.deposit
{
    public partial class approveInterestBulk : System.Web.UI.Page
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
                if (lblID != null && txtProposedAmount != null && chkSelected != null && chkSelected.Checked==true
                    && dtInterestDate!= null)
                {
                    int id = int.Parse(lblID.Text);
                    var pen = le.depositSchedules.FirstOrDefault(p => p.depositScheduleID == id);
                    if (pen != null)
                    {
                        pen.interestPayment = txtProposedAmount.Value.Value; 
                        pen.expensed = true;
                        pen.temp = false;
                        //pen.depositReference.Load();
                        pen.deposit.interestAccumulated += txtProposedAmount.Value.Value;
                        pen.deposit.interestBalance += txtProposedAmount.Value.Value;
                        //pen.deposit.depositTypeReference.Load();
                        //pen.deposit.clientReference.Load();
                        pen.repaymentDate = dtInterestDate.SelectedDate.Value;
                        pen.deposit.lastInterestDate = dtInterestDate.SelectedDate.Value;

                        var jb = journalextensions.Post("LN", pen.deposit.depositType.interestExpenseAccountID.Value,
                            pen.deposit.depositType.interestPayableAccountID.Value, (txtProposedAmount.Value.Value),
                            "Interest Calculated on Deposit - " + pen.deposit.client.surName + "," + pen.deposit.client.otherNames,
                            pro.currency_id.Value, pen.repaymentDate, pen.deposit.depositNo, ent, User.Identity.Name,
                            pen.deposit.client.branchID);

                        ent.jnl_batch.Add(jb);
                    }
                }
            }
            le.SaveChanges();
            ent.SaveChanges();
            HtmlHelper.MessageBox2("Interest on Deposit Data Saved Successfully!", ResolveUrl("~/ln/deposit/approveInterest.aspx"), "coreERP©: Successful", IconType.ok);  
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
                var lns2 = le.depositSchedules.Where(p => p.expensed==false && p.deposit.client.clientID==clientID).ToList();
                foreach (var ln in lns2)
                {
                    le.depositSchedules.Remove(ln);
                }
                var lns = le.depositSchedules.Where(p => p.deposit.principalBalance > 0 && (p.deposit.lastInterestDate == null || p.deposit.lastInterestDate < date)
                    && p.deposit.client.clientID == clientID).ToList();
                foreach (var ln in lns)
                {
                    //ln.depositReference.Load();
                    //ln.deposit.clientReference.Load(); 

                    //ln.deposit.depositTypeReference.Load();                    
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
            if (dtInterestDate1.SelectedDate != null)
            {
                CalculateInterest2(dtInterestDate1.SelectedDate.Value); 

                var lns = le.depositSchedules.Where(p=> p.expensed==false && p.deposit.principalBalance>0
                    && p.repaymentDate<=dtInterestDate1.SelectedDate.Value).ToList();
                foreach (var ln in lns)
                {
                    //ln.depositReference.Load();
                    //ln.deposit.clientReference.Load();

                    //ln.deposit.depositTypeReference.Load();
                }

                rpPenalty.DataSource = lns;
                rpPenalty.DataBind();
            }
        }

        public void CalculateInterest2(DateTime date)
        {
            try
            { 
                var dps = le.deposits.Where(p => p.principalBalance>0 && p.lastInterestDate<DateTime.Now.Date).ToList();

                var lns = le.depositSchedules.Where(p => p.deposit.principalBalance > 0 && p.expensed == false
                    ).ToList();


                foreach (var dp in dps)
                {
                    
                    List<DateTime> listInt = new List<DateTime>();
                    List<DateTime> listPrinc = new List<DateTime>();
                    List<DateTime> listAll = new List<DateTime>();

                    DateTime date2 = (dp.lastInterestDate == null) ? dp.firstDepositDate : dp.lastInterestDate.Value;
                    //dp.depositSchedules.Load();
                    DateTime lastDate=date2;
                    foreach (var sched in dp.depositSchedules.ToList())
                    {
                        if ((sched.repaymentDate >= date2 && sched.repaymentDate<=date)&&(sched.authorized==false)&&(sched.principalPayment==0 && sched.interestPayment==0))
                        {
                            le.depositSchedules.Remove(sched);
                        }
                        else if ((sched.repaymentDate >= date2 && sched.repaymentDate<=date) && (sched.principalPayment > 0)&& (sched.expensed==false))
                        {
                            sched.interestPayment = 0;
                        }
                        else if ((sched.repaymentDate >= date2 && sched.repaymentDate<=date)&&(sched.authorized==false)&&(sched.expensed==false))
                        {
                            le.depositSchedules.Remove(sched);
                        }
                    }
                    var totalPrinc = dp.amountInvested - dp.depositSchedules.Sum(p => p.principalPayment);
                    int i = 1;
                    var totalInt = dp.amountInvested * (date-date2).TotalDays/30.0 * (dp.interestRate) / 100.0;
                    var intererst = 0.0;
                    var princ = 0.0;
                    while (date2 < date)
                    {
                        date2 = date2.AddMonths(1);
                        if (date2 >= date) break;
                        if ((dp.interestRepaymentModeID == 30)
                            || (dp.interestRepaymentModeID == 90 && i % 3 == 0)
                            || (dp.interestRepaymentModeID == 180 && i % 6 == 0)
                            )
                        {
                            listInt.Add(date2);
                            if (listAll.Contains(date2) == false) listAll.Add(date2);
                        }
                        if ((dp.principalRepaymentModeID == 30)
                            || (dp.principalRepaymentModeID == 90 && i % 3 == 0)
                            || (dp.principalRepaymentModeID == 180 && i % 6 == 0)
                            )
                        {
                            listPrinc.Add(date2);
                            if (listAll.Contains(date2) == false) listAll.Add(date2);
                        }
                        i += 1;
                    }
                    listPrinc.Add(date);
                    listInt.Add(date);
                    listAll.Add(date);

                    if (dp.depositSchedules.FirstOrDefault(p => p.repaymentDate == date) != null)
                    {
                        dp.depositSchedules.FirstOrDefault(p => p.repaymentDate == date).interestPayment += intererst;
                    }
                    else
                    {
                        dp.depositSchedules.Add(new coreLogic.depositSchedule
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

    }
}