using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data.Entity;

namespace coreERP.ln.investment
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
                foreach (var cl in le.clients.Where(p => p.clientTypeID ==3).OrderBy(p => p.surName))
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                    ", " + cl.otherNames) + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }
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
                        var user = (new coreLogic.coreSecurityEntities()).users.First(p => p.user_name.ToLower().Trim() == User.Identity.Name.ToLower().Trim());
                        if (user.accessLevel.approvalLimit < txtProposedAmount.Value)
                        {
                            HtmlHelper.MessageBox("The amount to be approved is beyond your access level",
                                                        "coreERP©: Failed", IconType.deny);
                            return;
                        }

                        pen.interestPayment = txtProposedAmount.Value.Value; 
                        pen.expensed = true;
                        pen.temp = false; 
                        pen.investment.interestAccumulated += txtProposedAmount.Value.Value;
                        pen.investment.interestBalance += txtProposedAmount.Value.Value;
                        pen.repaymentDate = dtInterestDate.SelectedDate.Value;
                        pen.investment.lastInterestDate = dtInterestDate.SelectedDate.Value;

                        if (pen.interestPayment > 0)
                        {
                            pen.investment.investmentInterests.Add(new coreLogic.investmentInterest { 
                                creation_date=DateTime.Now,
                                creator=User.Identity.Name,
                                fromDate=pen.repaymentDate,
                                toDate=pen.repaymentDate,
                                interestAmount=pen.interestPayment,
                                interestBalance=pen.investment.interestBalance,
                                interestDate = pen.repaymentDate,
                                interestRate=pen.investment.interestRate,
                                localAmount=pen.interestPayment,
                                principal=pen.investment.principalBalance,
                                proposedAmount=0,
                                fxRate=pen.investment.fxRate
                            });
                        }

                        var jb = journalextensions.Post("LN", 
                            pen.investment.investmentType.interestReceivableAccountID.Value, 
                            pen.investment.investmentType.interestExpenseAccountID.Value,(txtProposedAmount.Value.Value),
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
            if (cboClient.SelectedValue != "" && this.dtInterestDate1.SelectedDate != null)
            {
                var id = -1;
                if (cboClient.SelectedValue != "")
                {
                    id = int.Parse(cboClient.SelectedValue);
                    CalculateInterest2(id, dtInterestDate1.SelectedDate.Value);
                }

                var startDate = dtInterestDate1.SelectedDate.Value.Date;
                var endDate = startDate.AddDays(1).AddSeconds(-1);
                var lns = le.investmentSchedules
                    .Include(p => p.investment)
                    .Include(p => p.investment.client)
                    .Where(p => p.expensed == false && (p.investment.principalBalance > 0 || p.investment.interestBalance > 0)
                    && p.investment.client.clientID == id && p.repaymentDate <= endDate
                    && (p.interestPayment > 1 || p.principalPayment > 1)).ToList();
                
                rpPenalty.DataSource = lns;
                rpPenalty.DataBind();

                foreach (RepeaterItem item in rpPenalty.Items)
                {
                    var lblID = item.FindControl("lblID") as Label;
                    var txtProposedAmount = item.FindControl("txtProposedAmount") as Telerik.Web.UI.RadNumericTextBox;
                    var dtInterestDate = item.FindControl("dtInterestDate") as Telerik.Web.UI.RadDatePicker;
                    var chkSelected = item.FindControl("chkSelected") as CheckBox;
                    if (lblID != null && txtProposedAmount != null && chkSelected != null && chkSelected.Checked == true
                        && dtInterestDate != null)
                    {
                        id = int.Parse(lblID.Text);
                        var ds = le.investmentSchedules.First(p => p.investmentScheduleID == id);
                        if (ds.investment.modern == true)
                        {
                            txtProposedAmount.Enabled = false;
                            dtInterestDate.Enabled = false;
                        }
                    }
                }
            }
        }

        protected string GetClientName(object investmentScheduleID)
        {
            string rtr = "";

            try
            {
                if (investmentScheduleID != null)
                {
                    var id = int.Parse(investmentScheduleID.ToString());
                    var ds = le.investmentSchedules.FirstOrDefault(p => p.investmentScheduleID == id);
                    if (ds != null && ds.investment != null && ds.investment.client != null)
                    {
                        if (ds.investment.client.companyName != null && ds.investment.client.companyName.Trim() != "")
                        {
                            rtr = ds.investment.client.companyName;
                        }
                        else if (ds.investment.client.accountName != null && ds.investment.client.accountName.Trim() != "")
                        {
                            rtr = ds.investment.client.accountName;
                        }
                        else
                        {
                            rtr = ds.investment.client.surName + ", " + ds.investment.client.otherNames;
                        }
                    }
                }
            }
            catch (Exception) { }

            return rtr;
        }

        public void CalculateInterest2(int clientID, DateTime date)
        {
            try
            { 
                var dps = le.investments.Where(p => p.client.clientID == clientID && p.principalBalance>0).ToList();

                var lns = le.investmentSchedules.Where(p => p.investment.principalBalance > 0 && p.expensed == false
                    && p.investment.client.clientID == clientID && p.repaymentDate <= date);

                foreach (var dp in dps)
                {
                    if (dp.modern == false)
                    {
                        List<DateTime> listInt = new List<DateTime>();
                        List<DateTime> listPrinc = new List<DateTime>();
                        List<DateTime> listAll = new List<DateTime>();

                        DateTime date2 = (dp.lastInterestDate == null) ? dp.firstinvestmentDate : dp.lastInterestDate.Value;
                        //dp.investmentSchedules.Load();
                        DateTime lastDate = date2;
                        foreach (var sched in dp.investmentSchedules.ToList())
                        {
                            if ((sched.repaymentDate >= date2 && sched.repaymentDate <= date) && (sched.principalPayment == 0) && (sched.expensed == false))
                            {
                                sched.interestPayment = 0;
                            }
                            else if ((sched.repaymentDate >= date2 && sched.repaymentDate <= date) && (sched.authorized == false) && (sched.expensed == false)
                                && (sched.temp == true))
                            {
                                le.investmentSchedules.Remove(sched);
                            }
                            if ((sched.repaymentDate >= date2 && sched.repaymentDate <= date) && (sched.authorized == false) && (sched.principalPayment == 0 &&
                                sched.interestPayment == 0))
                            {
                                le.investmentSchedules.Remove(sched);
                            }
                        }
                        var totalPrinc = dp.amountInvested - dp.investmentSchedules.Sum(p => p.principalPayment);
                        int i = 1;
                        var totalInt = dp.amountInvested * (date - date2).TotalDays / 30.0 * (dp.interestRate) / 100.0;
                        var intererst = 0.0;
                        var princ = 0.0;
                        DateTime maturityDate;
                        while (date2 < date)
                        {
                            //var localDate = date2;
                            date2 = date2.AddMonths(1);
                            maturityDate = date2;
                            if (date2 > dp.maturityDate.Value) maturityDate = dp.maturityDate.Value;
                            if (date2 >= date) break;

                            if ((dp.interestRepaymentModeID == 30)
                                || (dp.interestRepaymentModeID == 90 && i % 3 == 0)
                                || (dp.interestRepaymentModeID == 180 && i % 6 == 0)
                                )
                            {
                                listInt.Add(maturityDate);
                                if (listAll.Contains(maturityDate) == false) listAll.Add(maturityDate);
                            }
                            if ((dp.principalRepaymentModeID == 30)
                                || (dp.principalRepaymentModeID == 90 && i % 3 == 0)
                                || (dp.principalRepaymentModeID == 180 && i % 6 == 0)
                                )
                            {
                                listPrinc.Add(maturityDate);
                                if (listAll.Contains(maturityDate) == false) listAll.Add(maturityDate);
                            }
                            i += 1;
                        }
                        listPrinc.Add(date);
                        listInt.Add(date);
                        listAll.Add(date);

                        if (totalInt > 0)
                        {
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
                    }
                }

                le.SaveChanges();
            }
            catch (Exception x)
            {
                throw new ApplicationException(x.Message);
            }
        }

    }
}