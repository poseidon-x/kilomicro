using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using coreData.ErrorLog;
using coreLogic.HelperClasses;
using coreNotificationsDAL.Abstractions;
using coreNotificationsDAL.Helpers;
using coreNotificationsDAL;
using System.Configuration;

namespace coreLogic
{
    public class CashiersTillHelper : coreLogic.ICashiersTillHelper
    {
        private core_dbEntities ctx = new core_dbEntities();
        private ISmsDataHelper smsHelper;
        private const string MESSAGE_SENDER = "Kilo";
        private int SMS_HOUR_AFTER = int.Parse(ConfigurationManager.AppSettings["DISBURSEMENT_SMS_AFTER_HOUR"]);

        private string GetClientPhoneNo(int clientId)
        {
            string mobileNo = "";
            try
            {
                coreLoansEntities le = new coreLoansEntities();
                var phone = le.clientPhones.FirstOrDefault(e => e.clientID == clientId).phone;
                if (phone != null && phone.phoneTypeID == 2 && !string.IsNullOrWhiteSpace(phone.phoneNo))
                {
                    mobileNo = phone.phoneNo.Trim();
                }

            }
            catch (Exception e)
            {
                ExceptionManager.LogException(e, "CashiersTillHelper.GetClientPhoneNo");
            }
            return mobileNo;
        }

        private messageEvent PrepareDisbursementSmsEvent(cashierDisbursement r)
        {
            var newEvent = new messageEvent();
            try
            {
                //Get disbursement SMS template
                smsHelper = new SmsDataHelper();

                var smsTemplate = smsHelper.GetDisbursementSmsTemplate();

                // var smsBoby = Smart
                var smsMessageBody = smsTemplate
                    .Replace("$$FirstName$$", r.client.otherNames)
                    .Replace("$$AmountDisbursed$$", r.amount.ToString())
                    .Replace("$$DisbursementDate$$", r.txDate.ToString("dd-MMM-yyyy"));

                newEvent.clientID = r.clientID;
                newEvent.accountID = r.loanID;
                newEvent.eventDate = r.txDate.AddHours(SMS_HOUR_AFTER).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second);
                newEvent.eventID = r.cashierDisbursementID;
                newEvent.messageBody = smsMessageBody;
                newEvent.messageEventCategoryID = 9;
                newEvent.phoneNumber = GetClientPhoneNo(r.clientID);
                newEvent.sender = MESSAGE_SENDER;
                newEvent.finished = false;
            }
            catch (Exception e)
            {
                ExceptionManager.LogException(e, "CashiersTillHelper.PrepareDisbursementSmsEvent");

            }
            return newEvent;
        }

        public bool Post(string userName, DateTime dtStartDate, DateTime dtEndDate,
            Dictionary<string, List<int>> table)
        {

            coreLoansEntities le = new coreLoansEntities();


            var listDA = table["listDA"];
            var listDW = table["listDW"];
            var listDWC = table["listDWC"];
            var listSA = table["listSA"];
            var listSW = table["listSW"];
            var listReceipt = table["listReceipt"];
            var listDisb = table["listDisb"];
            var listIA = table["listIA"];
            var listCSC = table["listCSC"];
            var listIW = table["listIW"];
            var listSC = table["listSC"];
            var listSU = table["listSU"];
            var listRSC = table["listRSC"];
            var listRSU = table["listRSU"];

            CoreInfoLogger cil = new CoreInfoLogger();

            var u = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower() == userName.ToLower());
            if (u != null)
            {
                var day = le.cashiersTillDays.FirstOrDefault(p => p.tillDay <= dtEndDate
                && p.tillDay >= dtStartDate
                && p.cashiersTillID == u.cashiersTillID);
                if (day == null)
                {
                    throw new ApplicationException("Selected User has no till for Selected Day");
                }
                else
                {
                    coreLogic.CheckCoreDbEntities ent = new coreLogic.CheckCoreDbEntities();
                    coreLogic.IInvestmentManager invMgr = new coreLogic.InvestmentManager();
                    coreLogic.ISusuManager susuMgr = new coreLogic.SusuManager();
                    IRepaymentsManager rpmtMgr = new RepaymentsManager();
                    IRepaymentsManager rpmtMgrLink = new RepaymentsManagerLink();

                    IDisbursementsManager disbMgr = new DisbursementsManager();
                    var disb = le.cashierDisbursements
                        .Include(p => p.loan)
                        .Include(p => p.loan.client)
                        .Include(p => p.cashiersTill)
                        .Include(p => p.loan.client.loanGroupClients)
                        .Include(p => p.loan.client.loanGroupClients.Select(r => r.loanGroup))
                        .Where(p => p.txDate <= dtEndDate
                        && p.txDate >= dtStartDate
                        && p.cashiersTill.userName.ToLower()
                        == userName.ToLower() && p.posted == false)
                        .ToList();

                    if (disb.Any()) { cil.logInfo("Disbursing Cashier"); }
                    try
                    {
                        cil.logInfo("Loans Count " + listDisb.Count());

                        foreach (var r in disb)
                        {
                            cil.logInfo("cashier DisbursementID " + r.cashierDisbursementID);
                            if (listDisb.Contains(r.cashierDisbursementID) == true)
                            {
                                r.posted = true;

                                //schedule SMS event
                                var newEvent = PrepareDisbursementSmsEvent(r);
                                var isQueued= smsHelper.QueueSmsEvent(newEvent);

                                disbMgr.CashierDisburse(le, r.loan, ent, r, r.cashiersTill.userName);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        cil.logError(ex);
                        throw new ApplicationException("An error occured.");
                    }

                    var rcpt = le.cashierReceipts.Where(p => p.txDate <= dtEndDate
                        && p.txDate >= dtStartDate
                        && p.cashiersTill.userName.ToLower()
                        == userName.ToLower() && p.posted == false).ToList();

                    if (rcpt.Any()) { cil.logInfo("removing from cashier receipt"); }
                    try
                    {
                        for (int i = rcpt.Count - 1; i >= 0; i--)
                        {
                            var r = rcpt[i];
                            if (listReceipt.Contains(r.cashierReceiptID) == false)
                            {
                                rcpt.Remove(r);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        cil.logInfo(ex.InnerException.Message);
                    }

                    cil.logInfo("Posting Loan Repayment");

                    var comp = ctx.comp_prof.FirstOrDefault();
                    if (comp.comp_name.ToLower().Contains("link"))
                    {
                        Post(le, ent, rcpt, rpmtMgrLink, userName);
                    }
                    else
                    {
                        Post(le, ent, rcpt, rpmtMgr, userName);
                    }


                    var startDate = dtStartDate.Date;
                    var endDate = dtEndDate.Date.AddDays(1).AddSeconds(-1);

                    var das = le.depositAdditionals.Where(p => (p.depositDate >= startDate && p.depositDate <= endDate)
                        && (p.posted == false) && p.creator.ToLower() == userName.ToLower());

                    if (das.Any()) { cil.logInfo("Posting Additional Deposit"); }
                    try
                    {
                        foreach (var r in das)
                        {
                            if (listDA.Contains(r.depositAdditionalID) == true)
                            {
                                invMgr.PostDepositAdditional(r, r.creator, ent, le, u);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        cil.logInfo(ex.InnerException.Message);
                    }

                    var dws = le.depositWithdrawals.Where(p => (p.withdrawalDate >= startDate && p.withdrawalDate <= endDate)
                        && (p.posted == false) && p.creator.ToLower() == userName.ToLower());

                    if (dws.Any()) { cil.logInfo("Posting Deposit Withdrawals"); }
                    try
                    {
                        foreach (var r in dws)
                        {
                            if (listDW.Contains(r.depositWithdrawalID) == true)
                            {
                                invMgr.PostDepositsWithdrawal(r, r.creator, ent, le, u);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        cil.logInfo(ex.InnerException.Message);
                    }

                    var dwc = le.depositWithdrawals.Where(p => (p.withdrawalDate >= startDate && p.withdrawalDate <= endDate)
                        && p.disInvestmentCharge > 0 && (p.posted == false) && p.creator.ToLower() == userName.ToLower());

                    if (dwc.Any()) { cil.logInfo("Posting Deposit Withdrawals Charges"); }
                    try
                    {
                        foreach (var r in dwc)
                        {
                            if (listDWC.Contains(r.depositWithdrawalID) == true)
                            {
                                invMgr.PostDepositsWithdrawalCharges(r, r.creator, ent, le, u);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        cil.logInfo(ex.InnerException.Message);
                    }

                    var sas = le.savingAdditionals.Where(p => (p.savingDate >= startDate && p.savingDate <= endDate)
                        && (p.posted == false) && p.creator.ToLower() == userName.ToLower());

                    if (sas.Any()) { cil.logInfo("Posting Additional Savings"); }
                    try
                    {
                        foreach (var r in sas)
                        {
                            if (listSA.Contains(r.savingAdditionalID) == true)
                            {
                                invMgr.PostSavingAdditional(r, r.creator, ent, le, u);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        cil.logInfo(ex.InnerException.Message);
                    }

                    var sws = le.savingWithdrawals.Where(p => (p.withdrawalDate >= startDate && p.withdrawalDate <= endDate)
                        && (p.posted == false) && p.creator.ToLower() == userName.ToLower());

                    if (sws.Any()) { cil.logInfo("Posting Savings Withdrawals"); }
                    try
                    {
                        foreach (var r in sws)
                        {
                            if (listSW.Contains(r.savingWithdrawalID) == true)
                            {
                                invMgr.PostSavingsWithdrawal(r, r.creator, ent, le, u);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        cil.logInfo(ex.InnerException.Message);
                    }

                    var iws = le.investmentWithdrawals.Where(p => (p.withdrawalDate >= startDate && p.withdrawalDate <= endDate)
                        && (p.posted == false) && p.creator.ToLower() == userName.ToLower());

                    if (iws.Any()) { cil.logInfo("Posting Investment Withdrawals"); }
                    try
                    {
                        foreach (var r in iws)
                        {
                            if (listIW.Contains(r.investmentWithdrawalID) == true)
                            {
                                invMgr.PostInvestmentWithdrawal(r, r.creator, ent, le, u);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        cil.logInfo(ex.InnerException.Message);
                    }





                    var csc = le.clientServiceCharges.Where(p => (p.chargeDate >= startDate && p.chargeDate <= endDate)
                        && (p.posted == false) && p.creator.ToLower() == userName.ToLower());

                    if (csc.Any()) { cil.logInfo("Posting Client Service Charges"); }
                    try
                    {
                        foreach (var r in csc)
                        {
                            if (listCSC.Contains(r.clientServiceChargeId) == true)
                            {
                                invMgr.PostClientServiceCharge(r, r.creator, ent, le, u);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        cil.logInfo(ex.InnerException.Message);
                    }








                    var ias = le.investmentAdditionals.Where(p => (p.investmentDate >= startDate && p.investmentDate <= endDate)
                        && (p.posted == false) && p.creator.ToLower() == userName.ToLower());

                    if (ias.Any()) { cil.logInfo("Posting Investment Additionals"); }
                    try
                    {
                        foreach (var r in ias)
                        {
                            if (listIA.Contains(r.investmentAdditionalID) == true)
                            {
                                invMgr.PostInvestmentAdditional(r, r.creator, ent, le, u);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        cil.logInfo(ex.InnerException.Message);
                    }

                    var sus = le.susuAccounts.Where(p => (p.disbursementDate >= startDate && p.disbursementDate <= endDate)
                        && (p.authorized == true) && (p.posted == false) && p.disbursedBy != null && p.disbursedBy.ToLower() == userName.ToLower()).ToList();

                    if (sus.Any()) { cil.logInfo("Posting Susu Disbursement"); }
                    try
                    {
                        foreach (var r in sus)
                        {
                            if (listSU.Contains(r.susuAccountID) == true)
                            {
                                susuMgr.PostSusuDisbursement(le, ent, r, userName.ToLower());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        cil.logInfo(ex.InnerException.Message);
                    }

                    var scs = le.susuContributions.Where(p => (p.contributionDate >= startDate && p.contributionDate <= endDate)
                        && (p.posted == false) && p.cashierUsername != null
                            && p.cashierUsername.ToLower() == userName.ToLower()).ToList();

                    if (scs.Any()) { cil.logInfo("Posting Susu Contributions"); }
                    try
                    {
                        foreach (var r in scs)
                        {
                            if (listSC.Contains(r.susuContributionID) == true)
                            {
                                susuMgr.PostSusuContribution(le, ent, r, userName.ToLower());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        cil.logInfo(ex.InnerException.Message);
                    }


                    var rsus = le.regularSusuAccounts.Where(p => (p.disbursementDate >= startDate && p.disbursementDate <= endDate)
                       && (p.authorized == true) && (p.posted == false) && p.disbursedBy != null && p.disbursedBy.ToLower() == userName.ToLower()).ToList();

                    if (rsus.Any()) { cil.logInfo("Posting Regular Susu Disbursement"); }
                    try
                    {
                        foreach (var r in rsus)
                        {
                            if (listRSU.Contains(r.regularSusuAccountID) == true)
                            {
                                susuMgr.PostRegularSusuDisbursement(le, ent, r, userName.ToLower());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        cil.logInfo(ex.InnerException.Message);
                    }

                    var rscs = le.regularSusuContributions.Where(p => (p.contributionDate >= startDate && p.contributionDate <= endDate)
                        && (p.posted == false) && p.cashierUsername != null
                            && p.cashierUsername.ToLower() == userName.ToLower()).ToList();

                    if (rscs.Any()) { cil.logInfo("Posting Regular Susu Contributions"); }
                    try
                    {
                        foreach (var r in rscs)
                        {
                            if (listRSC.Contains(r.regularSusuContributionID) == true)
                            {
                                susuMgr.PostRegularSusuContribution(le, ent, r, userName.ToLower());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        cil.logInfo(ex.InnerException.Message);
                    }

                    if (ent.saveChangesWithChecks())
                    {
                        cil.logInfo("Saving Changes with checks");
                        le.SaveChanges();
                    }
                    else
                    {
                        throw new ApplicationException("The Batch is not balanced");
                    }


                    return true;
                }
            }
            else
            {
                throw new ApplicationException("No Till Defined for Selected User");
            }
        }

        private void Post(coreLoansEntities le, core_dbEntities ent, List<cashierReceipt> rcpts,
            IRepaymentsManager rpmtMgr,
            string userName)
        {
            List<string> loanIDs = new List<string>();

            //var distLoans2 = distClients.Where(p => p.Count > 1 || p.IsMulti==true);
            Dictionary<string, jnl_batch> lsBat = new Dictionary<string, jnl_batch>();
            var config = le.configs.FirstOrDefault();
            foreach (var r in rcpts)
            {
                var key = r.clientID.ToString();
                jnl_batch batch = null;
                if (lsBat.ContainsKey(key))
                {
                    batch = lsBat[key];
                }
                var comp = ctx.comp_prof.FirstOrDefault();

                if (r.paymentModeID == 1)//|| !comp.comp_name.ToLower().Contains("link"))
                {
                    if (r.paymentModeID == 1)
                    {
                        rpmtMgr.CashierReceipt(le, r.loan, r, ent, r.cashiersTill.userName);
                    }
                    else
                    {
                        var je = (new JournalExtensions());
                        rpmtMgr.CashierCheckReceipt(le, r.loan, r, ent, r.cashiersTill.userName, null, ref batch);
                    }
                }
                else
                {
                    RecieveBankLoanRepayment(le, ent, rpmtMgr, r, lsBat, loanIDs);
                }
                r.posted = true;
                if (r.paymentModeID != 1)
                {
                    r.closed = true;
                }
            }
        }

        private static void RecieveBankLoanRepayment(coreLoansEntities le, core_dbEntities ent, IRepaymentsManager rpmtMgr,
            cashierReceipt r, Dictionary<string, jnl_batch> lsBat, List<string> loanIDs)
        {
            int? crAccNo = null;
            var key = r.clientID.ToString();
            jnl_batch batch = null;
            if (lsBat.ContainsKey(key))
            {
                batch = lsBat[key];
            }
            multiPaymentClient mpc = (
                from m in le.multiPaymentClients
                from r2 in le.cashierReceipts
                where (m.cashierReceiptID == r2.cashierReceiptID
                       && r2.txDate == r.txDate
                       && r2.clientID == r.clientID)
                select m
                ).FirstOrDefault();
            int? ba = null;
            var b = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == r.bankID);
            if (b != null)
            {
                ba = b.accts.acct_id;
            }

            rpmtMgr.CashierCheckReceipt(le, r.loan, r, ent, r.cashiersTill.userName, ba, ref batch);

            var acc = ent.def_accts.FirstOrDefault(p => p.code == "RF");
            var pro = ent.comp_prof.FirstOrDefault();
            var je = (new JournalExtensions());

            if (acc != null)
            {
                crAccNo = acc.accts.acct_id;
                var js = batch.jnl.Where(p => p.accts.acct_id == ba && ba != null
                                              && p.description.Contains("Payment made by") == false)
                    .ToList();
                foreach (var j2 in js)
                {
                    ent.Entry(j2).State = System.Data.Entity.EntityState.Detached;
                }
            }

            if (mpc != null && mpc.posted == false /*&& loanIDs.Contains(key) == false  */)
            {
                PostBankAndRefundForRepayment(ent, r, loanIDs, key, je, ba, crAccNo, mpc, pro, batch);
            }
            else if (mpc == null && acc != null)
            {
                crAccNo = processRefundForRepayment(ent, r, loanIDs, crAccNo, acc, key, je, ba, pro, batch);
            }
            try
            {
                if (lsBat.Keys.Contains(key) == false)
                {
                    lsBat.Add(key, batch);
                }
            }
            catch (Exception)
            {
            }
        }

        private static void PostBankAndRefundForRepayment(core_dbEntities ent, cashierReceipt r, List<string> loanIDs, string key,
            JournalExtensions je, int? ba, int? crAccNo, multiPaymentClient mpc, comp_prof pro, jnl_batch batch)
        {
            loanIDs.Add(key);
            var jb = je.Post("LN2", ba.Value, crAccNo.Value,
                mpc.checkAmount,
                "Payment made by "
                + r.client.surName + ", " + r.client.otherNames, pro.currency_id.Value,
                r.txDate, r.loan.loanNo, ent, r.cashiersTill.userName, r.client.branchID);
            mpc.posted = true;
            var j = jb.jnl.FirstOrDefault(p => p.accts.acct_id == crAccNo);
            if (mpc.amount > 0)
            {
                j.crdt_amt = mpc.amount;
                ent.Entry(j).State = System.Data.Entity.EntityState.Detached;
            }
            else
            {
                j.crdt_amt = 0;
                ent.Entry(j).State = System.Data.Entity.EntityState.Detached;
            }
            foreach (var j2 in jb.jnl.ToList())
            {
                if ((j2.accts.acct_id == crAccNo && mpc.amount > 0)
                    || (j2.accts.acct_id != crAccNo))
                {
                    batch.jnl.Add(j2);
                }
            }
        }

        private static int? processRefundForRepayment(core_dbEntities ent, cashierReceipt r, List<string> loanIDs, int? crAccNo,
            def_accts acc, string key, JournalExtensions je, int? ba, comp_prof pro, jnl_batch batch)
        {
            crAccNo = acc.accts.acct_id;
            loanIDs.Add(key);
            var jb = je.Post("LN2", ba.Value, crAccNo.Value,
                r.amount, "Payment made by "
                          + r.client.surName + ", " + r.client.otherNames, pro.currency_id.Value,
                r.txDate, r.loan.loanNo, ent, r.cashiersTill.userName, r.client.branchID);
            var j = jb.jnl.FirstOrDefault(p => p.accts.acct_id == ba.Value);
            //j.dbt_amt = 0;
            foreach (var j2 in jb.jnl.ToList())
            {
                if (j2.accts.acct_id != crAccNo)
                {
                    batch.jnl.Add(j2);
                }
            }
            return crAccNo;
        }
    }
}
