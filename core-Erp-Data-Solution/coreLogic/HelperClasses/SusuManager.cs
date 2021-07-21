using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic
{
    public class SusuManager : ISusuManager
    {
        IJournalExtensions journalextensions = new JournalExtensions();

        public void PostSusuContribution(coreLoansEntities le, core_dbEntities ent, susuContribution sc,
            string userName)
        {
            var rem = sc.amount;
            foreach (var r in sc.susuAccount.susuContributionSchedules.Where(p => p.balance > 0).OrderBy(p => p.plannedContributionDate))
            {
                var amt = sc.susuAccount.contributionAmount;
                if (amt > rem)
                {
                    amt = rem;
                }
                if (amt <= 0) break;
                rem = rem - amt;
                r.balance = r.balance - amt;
                r.actualContributionDate = sc.contributionDate;
            }
            var conf = le.susuConfigs.FirstOrDefault();
            var lt = le.loanTypes.FirstOrDefault(p => p.loanTypeID == 7);
            if (lt == null) lt = le.loanTypes.FirstOrDefault();
            var pro = ent.comp_prof.FirstOrDefault();

            var proc = sc.susuAccount.commissionAmount - sc.susuAccount.commissionPaid;
            var it = sc.susuAccount.interestAmount - sc.susuAccount.interestPaid;
            var pr = sc.susuAccount.amountEntitled - proc - it - sc.susuAccount.principalPaid;
            var c = le.cashiersTills.FirstOrDefault(p => p.userName.Trim().ToLower() == userName.ToLower().Trim());
            int? acctID = null;

            if (c != null) acctID = c.accountID;
            else acctID = lt.vaultAccountID;

            var procAmt = (proc / sc.susuAccount.amountEntitled) * sc.amount;
            var itAmt = 0.0;
            if (it <= sc.amount - procAmt)
            {
                itAmt = it;
            }
            else
            {
                itAmt = sc.amount - procAmt;
            }
            var prAmt = sc.amount - itAmt - procAmt;

            if (procAmt > 0)
            {
                var jb3 = journalextensions.Post("LN", acctID.Value,
                    lt.commissionAndFeesAccountID, procAmt,
                    "Group Susu Commission for " + sc.susuAccount.client.surName + "," + sc.susuAccount.client.otherNames,
                    pro.currency_id.Value, sc.contributionDate, sc.susuAccount.susuAccountNo, ent, userName, sc.susuAccount.client.branchID);
                coreLogic.jnl_batch jb4 = journalextensions.Post("LN", lt.unpaidCommissionAccountID,
                    conf.contributionsPayableAccountID.Value, procAmt,
                    "Group Susu Commission for " + sc.susuAccount.client.surName + "," + sc.susuAccount.client.otherNames,
                    pro.currency_id.Value, sc.contributionDate, sc.susuAccount.susuAccountNo, ent, userName,
                    sc.susuAccount.client.branchID);
                var j = jb4.jnl.ToList();
                if (j.Count > 1)
                {
                    jb3.jnl.Add(j[0]);
                    jb3.jnl.Add(j[1]);
                }
                ent.jnl_batch.Add(jb3);
            }
            if (itAmt + prAmt > 0)
            { 
                coreLogic.jnl_batch jb = journalextensions.Post("LN", lt.unearnedInterestAccountID,
                        lt.interestIncomeAccountID, itAmt,
                        "Group Susu Contribution for " + sc.susuAccount.client.surName + "," + sc.susuAccount.client.otherNames,
                        pro.currency_id.Value, sc.contributionDate, sc.susuAccount.susuAccountNo, ent, userName,
                        sc.susuAccount.client.branchID);
                coreLogic.jnl_batch jb2 = journalextensions.Post("LN",
                   acctID.Value, conf.contributionsPayableAccountID.Value, itAmt + prAmt,
                    "Group Susu Contribution for " + sc.susuAccount.client.surName + "," + sc.susuAccount.client.otherNames,
                    pro.currency_id.Value, sc.contributionDate, sc.susuAccount.susuAccountNo, ent, userName,
                    sc.susuAccount.client.branchID);
                var j = jb2.jnl.ToList();
                if (j.Count > 1)
                {
                    jb.jnl.Add(j[0]);
                    jb.jnl.Add(j[1]);
                }
                ent.jnl_batch.Add(jb);
            }
            sc.appliedToLoan = true;
            sc.posted = true;
            if (sc.susuAccount.startDate == null)
            {
                sc.susuAccount.startDate = sc.contributionDate;
            }
        }

        public void PostRegularSusuContribution(coreLoansEntities le, core_dbEntities ent, regularSusuContribution sc,
            string userName)
        {
            var rem = sc.amount;
            foreach (var r in sc.regularSusuAccount.regularSusuContributionSchedules.Where(p => p.balance > 0).OrderBy(p => p.plannedContributionDate))
            {
                var amt = sc.regularSusuAccount.contributionAmount;
                if (amt > rem)
                {
                    amt = rem;
                }
                if (amt <= 0) break;
                rem = rem - amt;
                r.balance = r.balance - amt;
                r.actualContributionDate = sc.contributionDate;
            }
            var conf = le.susuConfigs.FirstOrDefault();
            var lt = le.loanTypes.FirstOrDefault(p => p.loanTypeID == 8);
            if (lt == null) lt = le.loanTypes.FirstOrDefault();
            var pro = ent.comp_prof.FirstOrDefault();

            var contrib = 0.0;
            var disb = 0.0;
            var comm = 0.0;
            var sa = sc.regularSusuAccount;
            if (sa.regularSusuContributions.Count > 0)
            {
                contrib = sa.regularSusuContributions.Sum(p => p.amount);
                comm = Math.Ceiling(sa.regularSusuContributions.Count / 31.0) * sa.contributionRate;
            }
            sa.regularSusCommissionAmount = comm;
            
            var proc = sa.regularSusCommissionAmount - sa.commissionPaid;
            var it = sa.interestAmount - sa.interestPaid;
            var pr = sa.amountEntitled - proc - it - sa.principalPaid;
            var c = le.cashiersTills.FirstOrDefault(p => p.userName.Trim().ToLower() == userName.ToLower().Trim());
            int? acctID = null;

            if (c != null) acctID = c.accountID;
            else acctID = lt.vaultAccountID;

            var periodsDone = (int)(Math.Ceiling((double)sc.regularSusuAccount.regularSusuContributions.Count / conf.regularSusuDaysInPeriod));
            var commissionsExpected = periodsDone * sc.regularSusuAccount.contributionAmount;
            var procAmt = commissionsExpected - sc.regularSusuAccount.commissionPaid;
            if (procAmt > sc.amount)
            {
                procAmt = sc.amount;
            }
            var itAmt = 0.0;
            if (it <= sc.amount - procAmt)
            {
                itAmt = it;
            }
            else
            {
                itAmt = sc.amount - procAmt;
            }
            var prAmt = sc.amount - itAmt - procAmt;

            if (procAmt > 0)
            {
                var jb3 = journalextensions.Post("LN", acctID.Value,
                    lt.commissionAndFeesAccountID, procAmt,
                    "Normal Susu Commission for " + sc.regularSusuAccount.client.surName + "," + sc.regularSusuAccount.client.otherNames,
                    pro.currency_id.Value, sc.contributionDate, sc.regularSusuAccount.regularSusuAccountNo, ent, userName, sc.regularSusuAccount.client.branchID);
                ent.jnl_batch.Add(jb3);
            }
            var amount = sc.amount - proc;
            if (amount > 0)
            {
                coreLogic.jnl_batch jb = journalextensions.Post("LN", lt.unearnedInterestAccountID,
                        lt.interestIncomeAccountID, itAmt,
                        "Normal Susu Contribution for " + sc.regularSusuAccount.client.surName + "," + sc.regularSusuAccount.client.otherNames,
                        pro.currency_id.Value, sc.contributionDate, sc.regularSusuAccount.regularSusuAccountNo, ent, userName, sc.regularSusuAccount.client.branchID);
                coreLogic.jnl_batch jb2 = journalextensions.Post("LN", conf.regularSusuContributionsPayableAccountID.Value,
                    acctID.Value, itAmt + prAmt,
                    "Normal Susu Contribution for " + sc.regularSusuAccount.client.surName + "," + sc.regularSusuAccount.client.otherNames,
                    pro.currency_id.Value, sc.contributionDate, sc.regularSusuAccount.regularSusuAccountNo, ent, userName, sc.regularSusuAccount.client.branchID);
                var j = jb2.jnl.ToList();
                if (j.Count > 1)
                {
                    jb.jnl.Add(j[0]);
                    jb.jnl.Add(j[1]);
                }
                ent.jnl_batch.Add(jb);
            }
            sc.appliedToLoan = true;
            sc.posted = true;
            if (sc.regularSusuAccount.startDate == null)
            {
                sc.regularSusuAccount.startDate = sc.contributionDate;
            }
        }

        public void PostSusuDisbursement(coreLoansEntities le, core_dbEntities ent, susuAccount sc,
            string userName)
        {

            var conf = le.susuConfigs.FirstOrDefault();
            sc.posted = true;
            var lt = le.loanTypes.FirstOrDefault(p => p.loanTypeID == 7);
            if (lt == null) lt = le.loanTypes.FirstOrDefault();
            var pro = ent.comp_prof.FirstOrDefault();
            var amountAtBank = sc.netAmountEntitled;
            var cr = le.cashiersTills.FirstOrDefault(p => p.userName.Trim().ToLower() == userName.ToLower().Trim());
            int? acctID = null;

            if (cr != null) acctID = cr.accountID;
            else acctID = lt.vaultAccountID;

            var jb = journalextensions.Post("S/S", conf.contributionsPayableAccountID.Value,
                    acctID.Value, amountAtBank,
                    "Group Susu Disbursement Principal - " + amountAtBank.ToString("#,###.#0")
                    + " - " + sc.client.accountNumber + " - " + sc.client.surName + "," + sc.client.otherNames,
                    pro.currency_id.Value, sc.disbursementDate.Value, sc.susuAccountNo, ent, userName, sc.client.branchID);
            var jb2 = journalextensions.Post("S/S", conf.contributionsPayableAccountID.Value,
                            lt.unpaidCommissionAccountID, sc.commissionAmount,
                            "Group Susu Disbursement Fees- " + sc.client.surName + "," + sc.client.otherNames,
                            pro.currency_id.Value, sc.disbursementDate.Value, sc.susuAccountNo, ent, userName, sc.client.branchID);
            var list = jb2.jnl.ToList();
            jb.jnl.Add(list[0]);
            jb.jnl.Add(list[1]);

            jb2 = journalextensions.Post("S/S", conf.contributionsPayableAccountID.Value,
                  lt.unearnedInterestAccountID, sc.interestAmount,
                         "Group Susu Disbursement Interest- " + sc.client.surName + "," + sc.client.otherNames,
                         pro.currency_id.Value, sc.disbursementDate.Value, sc.susuAccountNo, ent, userName, sc.client.branchID);
            jb.jnl.Add(list[0]);
            jb.jnl.Add(list[1]);

            foreach (var c in sc.susuContributions.Where(p => p.appliedToLoan == false).OrderBy(p => p.contributionDate))
            {
                var rem = c.amount;
                foreach (var r in sc.susuContributionSchedules.Where(p => p.balance > 0).OrderBy(p => p.plannedContributionDate))
                {
                    var amt = sc.contributionAmount;
                    if (amt > rem)
                    {
                        amt = rem;
                    }
                    if (amt <= 0) break;
                    rem = rem - amt;
                    r.balance = r.balance - amt;
                    r.actualContributionDate = c.contributionDate;
                }

                var proc = sc.commissionAmount - sc.commissionPaid;
                var it = sc.interestAmount - sc.interestPaid;
                var pr = sc.amountEntitled - proc - it - sc.principalPaid;
                var procAmt = (proc / c.susuAccount.amountEntitled) * c.amount;
                var itAmt = (it / c.susuAccount.amountEntitled) * c.amount;
                var prAmt = c.amount - itAmt - procAmt;

                if (procAmt > 0)
                {
                    var jb3 = journalextensions.Post("LN", acctID.Value,
                        lt.commissionAndFeesAccountID, procAmt,
                        "Group Susu Commission for " + sc.client.surName + "," + sc.client.otherNames,
                        pro.currency_id.Value, c.contributionDate, sc.susuAccountNo, ent, userName, sc.client.branchID);
                    coreLogic.jnl_batch jb4 = journalextensions.Post("LN", lt.unpaidCommissionAccountID,
                        conf.contributionsPayableAccountID.Value, procAmt,
                        "Group Susu Commission for " + sc.client.surName + "," + sc.client.otherNames,
                        pro.currency_id.Value, c.contributionDate, sc.susuAccountNo, ent, userName,
                    sc.client.branchID);
                    var j = jb4.jnl.ToList();
                    if (j.Count > 1)
                    {
                        jb3.jnl.Add(j[0]);
                        jb3.jnl.Add(j[1]);
                    }
                    ent.jnl_batch.Add(jb3);
                }
                if (itAmt + prAmt > 0)
                {
                    coreLogic.jnl_batch jb3 = journalextensions.Post("LN", lt.unearnedInterestAccountID,
                            lt.interestIncomeAccountID, itAmt,
                            "Group Susu Contribution for " + sc.client.surName + "," + sc.client.otherNames,
                            pro.currency_id.Value, c.contributionDate, sc.susuAccountNo, ent, userName,
                            sc.client.branchID);
                    coreLogic.jnl_batch jb4 = journalextensions.Post("LN",
                       acctID.Value, conf.contributionsPayableAccountID.Value, itAmt + prAmt,
                        "Group Susu Contribution for " + sc.client.surName + "," + sc.client.otherNames,
                        pro.currency_id.Value, c.contributionDate, sc.susuAccountNo, ent, userName,
                        sc.client.branchID);
                    var j = jb4.jnl.ToList();
                    if (j.Count > 1)
                    {
                        jb3.jnl.Add(j[0]);
                        jb3.jnl.Add(j[1]);
                    }
                    ent.jnl_batch.Add(jb3);
                }
                c.appliedToLoan = true;
            }
        }

        public void PostRegularSusuDisbursement(coreLoansEntities le, core_dbEntities ent, regularSusuAccount sc,
           string userName)
        {
            var conf = le.susuConfigs.FirstOrDefault();
            sc.posted = true;
            var lt = le.loanTypes.FirstOrDefault(p => p.loanTypeID == 8);
            if (lt == null) lt = le.loanTypes.FirstOrDefault();
            var pro = ent.comp_prof.FirstOrDefault();
            var amountAtBank = sc.netAmountEntitled;
            var cr = le.cashiersTills.FirstOrDefault(p => p.userName.Trim().ToLower() == userName.ToLower().Trim());
            int? acctID = null;

            if (cr != null) acctID = cr.accountID;
            else acctID = lt.vaultAccountID;

            var jb = journalextensions.Post("S/S", conf.regularSusuContributionsPayableAccountID.Value,
                    acctID.Value, amountAtBank,
                    "Normal Susu Disbursement Principal - " + amountAtBank.ToString("#,###.#0")
                    + " - " + sc.client.accountNumber + " - " + sc.client.surName + "," + sc.client.otherNames,
                    pro.currency_id.Value, sc.disbursementDate.Value, sc.regularSusuAccountNo, ent, userName, sc.client.branchID);
            var jb2 = journalextensions.Post("S/S", conf.regularSusuContributionsPayableAccountID.Value,
                            lt.unpaidCommissionAccountID, sc.regularSusCommissionAmount,
                            "Normal Susu Disbursement Fees- " + sc.client.surName + "," + sc.client.otherNames,
                            pro.currency_id.Value, sc.disbursementDate.Value, sc.regularSusuAccountNo, ent, userName, sc.client.branchID);
            var list = jb2.jnl.ToList();
            jb.jnl.Add(list[0]);
            jb.jnl.Add(list[1]);

            jb2 = journalextensions.Post("S/S", conf.regularSusuContributionsPayableAccountID.Value,
                  lt.unearnedInterestAccountID, sc.interestAmount,
                         "Normal Susu Disbursement Interest- " + sc.client.surName + "," + sc.client.otherNames,
                         pro.currency_id.Value, sc.disbursementDate.Value, sc.regularSusuAccountNo, ent, userName, sc.client.branchID);
            jb.jnl.Add(list[0]);
            jb.jnl.Add(list[1]);

            var contrib = 0.0;
                var disb = 0.0;
                var comm = 0.0;
                if (sc.regularSusuContributions.Count > 0)
                {
                    contrib = sc.regularSusuContributions.Sum(p => p.amount);
                    comm = Math.Ceiling(sc.regularSusuContributions.Count / 31.0) * sc.contributionRate;
                }
                sc.regularSusCommissionAmount = comm;
            
            foreach (var c in sc.regularSusuContributions.Where(p => p.appliedToLoan == false).OrderBy(p => p.contributionDate))
            {
                var rem = c.amount;
                foreach (var r in sc.regularSusuContributionSchedules.Where(p => p.balance > 0).OrderBy(p => p.plannedContributionDate))
                {
                    var amt = sc.contributionAmount;
                    if (amt > rem)
                    {
                        amt = rem;
                    }
                    if (amt <= 0) break;
                    rem = rem - amt;
                    r.balance = r.balance - amt;
                    r.actualContributionDate = c.contributionDate;
                }
                
                var proc = sc.regularSusCommissionAmount - sc.commissionPaid;
                var it = sc.interestAmount - sc.interestPaid;
                var pr = sc.amountEntitled - proc - it - sc.principalPaid;

                var procAmt = (proc / c.regularSusuAccount.amountEntitled) * c.amount;
                var itAmt = (it / c.regularSusuAccount.amountEntitled) * c.amount;
                var prAmt = c.amount - itAmt - procAmt;

                if (procAmt > 0)
                {
                    var jb3 = journalextensions.Post("LN", acctID.Value,
                        lt.commissionAndFeesAccountID, procAmt,
                        "Normal Susu Commission for " + sc.client.surName + "," + sc.client.otherNames,
                        pro.currency_id.Value, c.contributionDate, sc.regularSusuAccountNo, ent, userName, sc.client.branchID);
                    coreLogic.jnl_batch jb4 = journalextensions.Post("LN", lt.unpaidCommissionAccountID,
                        conf.regularSusuContributionsPayableAccountID.Value, procAmt,
                        "Normal Susu Commission for " + sc.client.surName + "," + sc.client.otherNames,
                        pro.currency_id.Value, c.contributionDate, sc.regularSusuAccountNo, ent, userName,
                    sc.client.branchID);
                    var j = jb4.jnl.ToList();
                    if (j.Count > 1)
                    {
                        jb3.jnl.Add(j[0]);
                        jb3.jnl.Add(j[1]);
                    }
                    ent.jnl_batch.Add(jb3);
                }
                if (itAmt + prAmt > 0)
                {
                    coreLogic.jnl_batch jb3 = journalextensions.Post("LN", lt.unearnedInterestAccountID,
                            lt.interestIncomeAccountID, itAmt,
                            "Normal Susu Contribution for " + sc.client.surName + "," + sc.client.otherNames,
                            pro.currency_id.Value, c.contributionDate, sc.regularSusuAccountNo, ent, userName,
                            sc.client.branchID);
                    coreLogic.jnl_batch jb4 = journalextensions.Post("LN", conf.regularSusuContributionsPayableAccountID.Value,
                       acctID.Value, itAmt + prAmt,
                        "Normal Susu Contribution for " + sc.client.surName + "," + sc.client.otherNames,
                        pro.currency_id.Value, c.contributionDate, sc.regularSusuAccountNo, ent, userName,
                        sc.client.branchID);
                    var j = jb4.jnl.ToList();
                    if (j.Count > 1)
                    {
                        jb3.jnl.Add(j[0]);
                        jb3.jnl.Add(j[1]);
                    }
                    ent.jnl_batch.Add(jb3);
                }
                c.appliedToLoan = true;
            }
        }

    }
}
