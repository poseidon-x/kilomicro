using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.investment
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
                    var pen = le.investmentSchedules.FirstOrDefault(p => p.investmentScheduleID == id);
                    if (pen != null)
                    {
                        pen.interestPayment = txtProposedAmount.Value.Value; 
                        pen.expensed = true;
                        pen.temp = false;
                        //pen.investmentReference.Load();
                        pen.investment.interestAccumulated += txtProposedAmount.Value.Value;
                        pen.investment.interestBalance += txtProposedAmount.Value.Value;
                        //pen.investment.investmentTypeReference.Load();
                        //pen.investment.clientReference.Load();
                        pen.repaymentDate = dtInterestDate.SelectedDate.Value;
                        pen.investment.lastInterestDate = dtInterestDate.SelectedDate.Value;

                        var jb = journalextensions.Post("LN", pen.investment.investmentType.interestExpenseAccountID.Value,
                            pen.investment.investmentType.interestReceivableAccountID.Value, (txtProposedAmount.Value.Value),
                            "Interest Calculated on Deposit - " + pen.investment.client.surName + "," + pen.investment.client.otherNames,
                            pro.currency_id.Value, pen.repaymentDate, pen.investment.investmentNo, ent, User.Identity.Name,
                            pen.investment.client.branchID);

                        ent.jnl_batch.Add(jb);
                    }
                }
            }
            le.SaveChanges();
            ent.SaveChanges();
            HtmlHelper.MessageBox2("Interest on Deposit Data Saved Successfully!", ResolveUrl("~/ln/investment/approveInterest.aspx"), "coreERP©: Successful", IconType.ok);  
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
                var lns2 = le.investmentSchedules.Where(p => p.expensed==false && p.investment.client.clientID==clientID).ToList();
                foreach (var ln in lns2)
                {
                    le.investmentSchedules.Remove(ln);
                }
                var lns = le.investmentSchedules.Where(p => p.investment.principalBalance > 0 && (p.investment.lastInterestDate == null || p.investment.lastInterestDate < date)
                    && p.investment.client.clientID == clientID).ToList();
                foreach (var ln in lns)
                {
                    //ln.investmentReference.Load();
                    //ln.investment.clientReference.Load(); 

                    //ln.investment.investmentTypeReference.Load();                    
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

                var lns = le.investmentSchedules.Where(p=> p.expensed==false && p.investment.principalBalance>0
                    && p.repaymentDate<=dtInterestDate1.SelectedDate.Value).ToList();
                foreach (var ln in lns)
                {
                    //ln.investmentReference.Load();
                    //ln.investment.clientReference.Load();

                    //ln.investment.investmentTypeReference.Load();
                }

                rpPenalty.DataSource = lns;
                rpPenalty.DataBind();
            }
        }

        public void CalculateInterest2(DateTime date)
        {
            try
            { 
                var dps = le.investments.Where(p => p.principalBalance>0 && p.lastInterestDate<DateTime.Now.Date).ToList();

                var lns = le.investmentSchedules.Where(p => p.investment.principalBalance > 0 && p.expensed == false
                    ).ToList();


                foreach (var dp in dps)
                {
                    
                    List<DateTime> listInt = new List<DateTime>();
                    List<DateTime> listPrinc = new List<DateTime>();
                    List<DateTime> listAll = new List<DateTime>();

                    DateTime date2 = (dp.lastInterestDate == null) ? dp.firstinvestmentDate : dp.lastInterestDate.Value;
                    //dp.investmentSchedules.Load();
                    DateTime lastDate=date2;
                    foreach (var sched in dp.investmentSchedules.ToList())
                    {
                        if ((sched.repaymentDate >= date2 && sched.repaymentDate<=date)&&(sched.authorized==false)&&(sched.principalPayment==0 && sched.interestPayment==0))
                        {
                            le.investmentSchedules.Remove(sched);
                        }
                        else if ((sched.repaymentDate >= date2 && sched.repaymentDate<=date) && (sched.principalPayment > 0)&& (sched.expensed==false))
                        {
                            sched.interestPayment = 0;
                        }
                        else if ((sched.repaymentDate >= date2 && sched.repaymentDate<=date)&&(sched.authorized==false)&&(sched.expensed==false))
                        {
                            le.investmentSchedules.Remove(sched);
                        }
                    }
                    var totalPrinc = dp.amountInvested - dp.investmentSchedules.Sum(p => p.principalPayment);
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

                    if (dp.investmentSchedules.FirstOrDefault(p => p.repaymentDate == date) != null)
                    {
                        dp.investmentSchedules.FirstOrDefault(p => p.repaymentDate == date).interestPayment += intererst;
                    }
                    else
                    {
                        dp.investmentSchedules.Add(new coreLogic.investmentSchedule
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