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
    public partial class rollOver : System.Web.UI.Page
    {
        IJournalExtensions journalextensions = new JournalExtensions();
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            if(!IsPostBack)
            {
                cboClient.Items.Add(new RadComboBoxItem("", ""));
                foreach (var cl in le.clients.OrderBy(p=> p.surName))
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                    ", " + cl.otherNames) + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }
            }
        }
  
        protected void cboClient_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboClient.SelectedValue != "")
            {
                int clientID = int.Parse(cboClient.SelectedValue);
                cboSavings.Items.Clear();
                cboSavings.Items.Add(new RadComboBoxItem("", ""));
                foreach (var cl in le.savings.Where(p => p.client.clientID == clientID))
                {
                    cboSavings.Items.Add(new RadComboBoxItem(cl.amountInvested.ToString("#,###.#0")
                        + " ("+cl.firstSavingDate.ToString("dd-MMM-yyyy") +")", cl.savingID.ToString()));
                }
            }
        }

        protected void cboSavings_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboSavings.SelectedValue != "")
            {
                int savingID = int.Parse(cboSavings.SelectedValue);
                var dp = le.savings.FirstOrDefault(p => p.savingID == savingID);
                if (dp != null)
                {
                    lblAmount.Text = dp.amountInvested.ToString("#,###.#0");
                    lblDepDate.Text = dp.firstSavingDate.ToString("dd-MMM-yyyy");
                    lblInte.Text = dp.interestBalance.ToString("#,###.#0");
                    lblMaturityDate.Text = dp.maturityDate.Value.ToString("dd-MMM-yyyy");
                    lblPrinc.Text = dp.principalBalance.ToString("#,###.#0"); 
                    if (dp.maturityDate.Value < DateTime.Now)
                    { 
                        btnRollOverInt.Enabled = true; 
                    }
                    else
                    { 
                        btnRollOverInt.Enabled = false; 
                    }
                }
            }
        } 

        protected void btnRollOverInt_Click(object sender, EventArgs e)
        {
            if (cboSavings.SelectedValue != "" && dtAppDate.SelectedDate!= null)
            {
                int savingID = int.Parse(cboSavings.SelectedValue);
                var dp = le.savings.FirstOrDefault(p => p.savingID == savingID);
                if (dp != null)
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
                    dp.maturityDate = dtAppDate.SelectedDate.Value.AddMonths((int)txtPeriod.Value.Value);
                    dp.period = (int)txtPeriod.Value.Value;
                    le.SaveChanges();
                    ent.SaveChanges();
                    HtmlHelper.MessageBox2("Savings Account Rolled Over Successfully", ResolveUrl("~/ln/saving/rollOver.aspx"), "coreERP©: Successful", IconType.ok);

                }
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