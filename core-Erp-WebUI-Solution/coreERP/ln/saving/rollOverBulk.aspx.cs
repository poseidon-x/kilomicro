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
    public partial class rollOverBulk : System.Web.UI.Page
    {
        IJournalExtensions journalextensions = new JournalExtensions();
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            if(!IsPostBack)
            { 
            }
        }

        protected void btnRollOverInt_Click(object sender, EventArgs e)
        {
            if (dtAppDate.SelectedDate!= null)
            {
                var dps = le.savings.Where(p => p.maturityDate >= dtAppDate.SelectedDate.Value).ToList();
                foreach(var dp in dps)
                {
                    if (dp.lastInterestDate == null || dp.lastInterestDate < dtAppDate.SelectedDate.Value.Date)
                    {
                        CalculateInterest2(dp, dtAppDate.SelectedDate.Value.Date);
                    }
                    var iamount = dp.interestBalance;
                    var pamount = 0;
                     
                    dp.principalBalance += dp.interestBalance;
                    dp.savingRollOvers.Add(new coreLogic.savingRollOver { 
                        interestPayment=dp.interestBalance,
                        principalPayment=dp.principalBalance,
                        rollOverDate=dtAppDate.SelectedDate.Value                       
                    });
                    dp.interestBalance = 0;
                    dp.modification_date = DateTime.Now;
                    dp.last_modifier = User.Identity.Name;
                    dp.maturityDate = dtAppDate.SelectedDate.Value.AddMonths(dp.period); 
                }
                le.SaveChanges();
                ent.SaveChanges();
                HtmlHelper.MessageBox2("Savings Accounts Rolled Over Successfully", ResolveUrl("~/ln/saving/rollOver.aspx"), "coreERP©: Successful", IconType.ok);

            }
        }

        public void CalculateInterest2(coreLogic.saving dp, DateTime date)
        {
            try
            {

                List<DateTime> listInt = new List<DateTime>();
                List<DateTime> listPrinc = new List<DateTime>();
                List<DateTime> listAll = new List<DateTime>();

                DateTime date2 = (dp.lastInterestDate == null) ? dp.firstSavingDate : dp.lastInterestDate.Value;
                //dp.savingSchedules.Load();
                DateTime lastDate = date2;
                foreach (var sched in dp.savingSchedules.ToList())
                {
                    if ((sched.repaymentDate >= date2 && sched.repaymentDate <= date) && (sched.authorized == false) && (sched.principalPayment == 0 && sched.interestPayment == 0))
                    {
                        le.savingSchedules.Remove(sched);
                    }
                    else if ((sched.repaymentDate >= date2 && sched.repaymentDate <= date) && (sched.principalPayment > 0) && (sched.expensed == false))
                    {
                        sched.interestPayment = 0;
                    }
                    else if ((sched.repaymentDate >= date2 && sched.repaymentDate <= date) && (sched.authorized == false) && (sched.expensed == false))
                    {
                        le.savingSchedules.Remove(sched);
                    }
                }
                var totalPrinc = dp.amountInvested - dp.savingSchedules.Sum(p => p.principalPayment);
                int i = 1;
                var totalInt = dp.principalBalance * (date - date2).TotalDays / 30.0 * (dp.interestRate) / 100.0;
                var intererst = totalInt;
                var princ = 0.0;

                var pro = ent.comp_prof.FirstOrDefault();
                coreLogic.savingSchedule pen = null;
                if (dp.savingSchedules.FirstOrDefault(p => p.repaymentDate == date) != null)
                {
                    pen = dp.savingSchedules.FirstOrDefault(p => p.repaymentDate == date);
                    pen.interestPayment += intererst;
                }
                else
                {
                    pen = new coreLogic.savingSchedule
                    {
                        interestPayment = totalInt,
                        principalPayment = 0,
                        repaymentDate = date,
                        authorized = false,
                        temp = true
                    };
                    dp.savingSchedules.Add(pen);
                }
                pen.interestPayment = intererst;
                pen.expensed = true;
                pen.temp = false;
                //pen.savingReference.Load();
                pen.saving.interestAccumulated += intererst;
                pen.saving.interestBalance += intererst;
                //pen.saving.savingTypeReference.Load();
                //pen.saving.clientReference.Load();
                pen.repaymentDate = date;
                pen.saving.lastInterestDate = date;

                var jb = journalextensions.Post("LN", pen.saving.savingType.interestExpenseAccountID.Value,
                    pen.saving.savingType.accountsPayableAccountID.Value, intererst,
                    "Interest Calculated on Savings - " + pen.saving.client.surName + "," + pen.saving.client.otherNames,
                    pro.currency_id.Value, pen.repaymentDate, pen.saving.savingNo, ent, User.Identity.Name,
                                pen.saving.client.branchID);

                ent.jnl_batch.Add(jb);
            }
            catch (Exception x)
            {
            }
        }

    }
}