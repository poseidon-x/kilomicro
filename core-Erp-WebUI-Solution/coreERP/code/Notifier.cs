using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreNotificationsDAL;
using Microsoft.AspNet.SignalR;
using Quartz;
using System.IO;

namespace coreERP
{
    [DisallowConcurrentExecution]
    public class Notifier:IJob
    {
        private static string MaturedInvestmentTemplate = "";
        private static string NotificationsContainerTemplate = "";
        private static string NoteHeaderTemplate = "";
        private static string NoteBodyTemplate = "";
        private static string NoteGroupTemplate = "";
        private static string InvestmentDuePaymentTemplate = "";
        private static string LoanDuePaymentTemplate = "";
        private static string LoanUnapprovedTemplate = "";
        private static string LoanUndisbursedTemplate = "";
        private static string ClientActivityTemplate = "";
        private static string OpenTillDaysTemplate = "";
        private volatile bool executing = false;

        private static int MATURED_INVESTMENT_NOTE_TYPE_ID = 1;
        private static int INVESTMENT_PAYMENT_DUE_NOTE_TYPE_ID = 2;
        private static int LOAN_PAYMENT_DUE_NOTE_TYPE_ID = 3;
        private static int LOAN_UNAPPROVED_NOTE_TYPE_ID = 4;
        private static int LOAN_UNDISBURSED_NOTE_TYPE_ID = 5;
        private static int CLIENT_ACTIVITY_NOTE_TYPE_ID = 6;
        private static int OPEN_TILL_DAYS_NOTE_TYPE_ID = 7;

        static Notifier()
        {
            MaturedInvestmentTemplate = Notifier.LoadFile(
                Path.Combine(HttpRuntime.AppDomainAppPath,
                "Content\\templates\\maturedInvestment.html"));
            NotificationsContainerTemplate = Notifier.LoadFile(
                Path.Combine(HttpRuntime.AppDomainAppPath,
                "Content\\templates\\notificationsContainer.html"));
            NoteHeaderTemplate = Notifier.LoadFile(
                Path.Combine(HttpRuntime.AppDomainAppPath,
                "Content\\templates\\noteHeader.html"));
            NoteBodyTemplate = Notifier.LoadFile(
                Path.Combine(HttpRuntime.AppDomainAppPath,
                "Content\\templates\\noteBody.html"));
            NoteGroupTemplate = Notifier.LoadFile(
                Path.Combine(HttpRuntime.AppDomainAppPath,
                "Content\\templates\\noteGroup.html"));
            InvestmentDuePaymentTemplate = Notifier.LoadFile(
                Path.Combine(HttpRuntime.AppDomainAppPath,
                "Content\\templates\\investmentDuePayment.html"));
            LoanUnapprovedTemplate = Notifier.LoadFile(
                Path.Combine(HttpRuntime.AppDomainAppPath,
                "Content\\templates\\unapprovedLoan.html"));
            LoanUndisbursedTemplate = Notifier.LoadFile(
                Path.Combine(HttpRuntime.AppDomainAppPath,
                "Content\\templates\\undisbursedLoan.html"));
            ClientActivityTemplate = Notifier.LoadFile(
                Path.Combine(HttpRuntime.AppDomainAppPath,
                "Content\\templates\\clientActivityLog.html"));
            LoanDuePaymentTemplate = Notifier.LoadFile(
                Path.Combine(HttpRuntime.AppDomainAppPath,
                "Content\\templates\\loansDue.html"));
            OpenTillDaysTemplate = Notifier.LoadFile(
                Path.Combine(HttpRuntime.AppDomainAppPath,
                "Content\\templates\\openDays.html"));
        }

        public void Execute(IJobExecutionContext context)
        {
            try 
            {
                if (executing) return;
                executing = true;
                var userMessages = new Dictionary<string, Note>();
                using (var ne = new notificationsModel())
                {
                    using (var le = new coreLogic.coreLoansEntities())
                    {
                        //Matured Investments
                        var notes = new Dictionary<string, Note>();
                        GetMaturedInvestmentsNotes(ne, le, notes);
                        CopyMessages(notes, userMessages);

                        //Due Investents Repayments
                        notes = new Dictionary<string, Note>();
                        GetInvestmentPaymentsDueNotes(ne, le, notes);
                        CopyMessages(notes, userMessages);

                        //Due Loan Repayments
                        notes = new Dictionary<string, Note>();
                        //GetLoanPaymentsDueNotes(ne, le, notes);
                        //CopyMessages(notes, userMessages);

                        //Unapproved Loans
                        notes = new Dictionary<string, Note>();
                        GetLoanUnapprovedNotes(ne, le, notes);
                        CopyMessages(notes, userMessages);

                        //Undisbursed Loans
                        notes = new Dictionary<string, Note>();
                        GetLoanUndisbursedNotes(ne, le, notes);
                        CopyMessages(notes, userMessages);

                        //Client Activity
                        notes = new Dictionary<string, Note>();
                        GetClientActivityNotes(ne, le, notes);
                        CopyMessages(notes, userMessages);

                        //Open Cashiers' Till
                        notes = new Dictionary<string, Note>();
                        GetOpenTillDaysNotes(ne, le, notes);
                        CopyMessages(notes, userMessages);
                    }
                }

                sendMessages(userMessages);
            }
            catch (Exception) { }
            executing = false;
        }

        private void CopyMessages(Dictionary<string, Note> source, Dictionary<string, Note> dest)
        {
            foreach (var msg in source)
            {
                if (dest.ContainsKey(msg.Key))
                {
                    dest[msg.Key] = new Note
                    {
                        User = msg.Key,
                        Header = dest[msg.Key].Header + msg.Value.Header.Replace("$$ACTIVE$$", ""),
                        Body = dest[msg.Key].Body + msg.Value.Body,
                        NoOfMessages = dest[msg.Key].NoOfMessages + msg.Value.NoOfMessages
                    };
                }
                else
                {
                    dest.Add(msg.Key, new Note
                    {
                        User = msg.Key,
                        Header = msg.Value.Header.Replace("$$ACTIVE$$", "k-state-active"),
                        Body = msg.Value.Body,
                        NoOfMessages = msg.Value.NoOfMessages
                    });
                }
            }
        }
        private void GetMaturedInvestmentsNotes(notificationsModel ne, coreLogic.coreLoansEntities le,
            Dictionary<string,Note> userMessages)
        { 
            try
            {
                //Not 1 -- Matured Investments
                var matured = le.deposits
                    .Where(p => p.maturityDate <= DateTime.Now && p.principalBalance + p.interestBalance > 2)
                    .ToList();
                var messages = new List<string>();
                var staffMessages = new Dictionary<string, List<string>>();
                var msgs = new List<string>();
                foreach (var mat in matured)
                {
                    var message = String.Format(MaturedInvestmentTemplate,
                        "/ln/loans/image.aspx?cid=" + mat.client.clientID.ToString() + "&h=48&w=48",
                        ((mat.client.clientCompany != null) ? mat.client.clientCompany.companyName
                            : (mat.client.accountName != null && mat.client.accountName.Trim().Length > 4 ? mat.client.accountName :
                            mat.client.surName + ", " + mat.client.otherNames)),
                        "/ln/deposit/with.aspx?id=" + mat.depositID.ToString(),
                        mat.depositNo,
                        (mat.principalBalance + mat.interestBalance).ToString("#,##0.#0"),
                        mat.maturityDate.Value.ToString("dd-MMM-yyyy"),
                        "/ln/deposit/rollOver.aspx?id=" + mat.depositID.ToString());

                    var staff = le.staffs.FirstOrDefault(p => p.staffID == mat.staffID);
                    if(staff != null && staff.userName != null)
                    {
                        message = message.Replace("NOT DEFINED", staff.surName + ", " + staff.otherNames);
                        if (staffMessages.ContainsKey(staff.userName))
                        {
                            staffMessages[staff.userName].Add(message);
                        }
                        else
                        {
                            staffMessages.Add(staff.userName, new List<string> { message });
                        }

                    }
                    msgs.Add(message);
                } 
                notifyUsers(ne, msgs, userMessages, "Matured Investments", MATURED_INVESTMENT_NOTE_TYPE_ID, null, staffMessages.Keys);
                if (staffMessages.Count > 0)
                {
                    foreach (var msg in staffMessages)
                    {
                        notifyUsers(ne, messages, userMessages, "Matured Investments", MATURED_INVESTMENT_NOTE_TYPE_ID,
                            new List<string> { msg.Key }, staffMessages.Keys);
                    }
                }
            }
            catch (Exception) { }  
        }

        private void GetInvestmentPaymentsDueNotes(notificationsModel ne, coreLogic.coreLoansEntities le,
            Dictionary<string, Note> userMessages)
        {
            try
            {
                var oneMonthAgo = DateTime.Now.AddDays(-30);
                //Not 1 -- Matured Investments
                var matured = le.deposits
                    .Where(p => (p.principalRepaymentModeID==30 || p.interestRepaymentModeID==30)  
                            && p.principalBalance + p.interestBalance > 2)
                    .ToList();
                var messages = new List<string>();
                var staffMessages = new Dictionary<string, List<string>>();
                var msgs = new List<string>();
                foreach (var mat in matured)
                {
                    var lastPayment = mat.depositWithdrawals.OrderByDescending(prop => prop.withdrawalDate)
                        .FirstOrDefault();
                    var lastPaymentDate = mat.firstDepositDate;
                    if (lastPayment != null) lastPaymentDate = lastPayment.withdrawalDate;
                    if (lastPaymentDate >= oneMonthAgo) continue;
                    var message = String.Format(InvestmentDuePaymentTemplate,
                        "/ln/loans/image.aspx?cid=" + mat.client.clientID.ToString() + "&h=48&w=48",
                        ((mat.client.clientCompany != null) ? mat.client.clientCompany.companyName
                            : (mat.client.accountName != null && mat.client.accountName.Trim().Length > 4 ? mat.client.accountName :
                            mat.client.surName + ", " + mat.client.otherNames)),
                        "/ln/deposit/with.aspx?id=" + mat.depositID.ToString(),
                        mat.depositNo,
                        (mat.principalBalance + mat.interestBalance).ToString("#,##0.#0"),
                        lastPaymentDate.ToString("dd-MMM-yyyy") );

                    var staff = le.staffs.FirstOrDefault(p => p.staffID == mat.staffID);
                    if(staff != null && staff.userName != null)
                    {
                        message = message.Replace("NOT DEFINED", staff.surName + ", " + staff.otherNames);
                        if (staffMessages.ContainsKey(staff.userName))
                        {
                            staffMessages[staff.userName].Add(message);
                        }
                        else
                        {
                            staffMessages.Add(staff.userName, new List<string> { message });
                        }

                    }
                    msgs.Add(message);
                }
                notifyUsers(ne, msgs, userMessages, "Due Interest Pmts", INVESTMENT_PAYMENT_DUE_NOTE_TYPE_ID, null, staffMessages.Keys);
                if (staffMessages.Count > 0)
                {
                    foreach (var msg in staffMessages)
                    {
                        notifyUsers(ne, messages, userMessages, "Due Interest Pmts", INVESTMENT_PAYMENT_DUE_NOTE_TYPE_ID,
                            new List<string> { msg.Key }, staffMessages.Keys);
                    }
                }
            }
            catch (Exception) { }
        }

        private void GetLoanPaymentsDueNotes(notificationsModel ne, coreLogic.coreLoansEntities le,
            Dictionary<string, Note> userMessages)
        {
            try
            {
                var oneMonthAgo = DateTime.Now.AddDays(-30);
                //Not 1 -- Matured Investments
                var matured = le.loans
                    .Where(p => (p.balance > 2 && p.loanStatusID == 4))
                    .ToList();
                var messages = new List<string>();
                var staffMessages = new Dictionary<string, List<string>>();
                var msgs = new List<string>();
                foreach (var mat in matured)
                {
                    var totalDue = mat.repaymentSchedules.Sum(p => p.principalBalance + p.interestBalance);
                    var amountDue = mat.repaymentSchedules.Where(p => p.repaymentDate <= DateTime.Now)
                        .Sum(p => p.principalBalance + p.interestBalance);
                    var lastPayment = mat.loanRepayments
                        .Where(p=> p.repaymentTypeID != 6)//Exclude Processing Fee
                        .OrderByDescending(p => p.repaymentDate)
                        .FirstOrDefault();
                    var lastPaymentDate = mat.disbursementDate;
                    if (lastPayment != null) lastPaymentDate = lastPayment.repaymentDate;
                    if (amountDue < 9 ) continue;
                    var message = String.Format(LoanDuePaymentTemplate,
                        "/ln/loans/image.aspx?cid=" + mat.client.clientID.ToString() + "&h=48&w=48",
                        ((mat.client.clientCompany != null) ? mat.client.clientCompany.companyName
                            : (mat.client.accountName != null && mat.client.accountName.Trim().Length > 4 ? mat.client.accountName :
                            mat.client.surName + ", " + mat.client.otherNames)),
                        "/ln/loans/loan.aspx?id=" + mat.loanID.ToString(),
                        mat.loanNo,
                        amountDue.ToString("#,##0.#0"),
                        totalDue.ToString("#,##0.#0"),
                        ((lastPaymentDate == null) ? "NO PAYMENTS MADE" : lastPaymentDate.Value.ToString("dd-MMM-yyyy")),
                        mat.disbursementDate.Value.AddMonths((int)mat.loanTenure).ToString("dd-MMM-yyyy"));
                    if (mat.staffID != null && mat.staff == null && mat.staff.userName != null)
                    {
                        message = message.Replace("NOT DEFINED", mat.staff.surName + ", " + mat.staff.otherNames);
                        if (staffMessages.ContainsKey(mat.staff.userName))
                        {
                            staffMessages[mat.staff.userName].Add(message);
                        }
                        else
                        {
                            staffMessages.Add(mat.staff.userName, new List<string>{message});
                        }
                        
                    }
                    msgs.Add(message);
                }
                notifyUsers(ne, msgs, userMessages, "Due Loan Pmts", LOAN_PAYMENT_DUE_NOTE_TYPE_ID, null, staffMessages.Keys);
                if (staffMessages.Count > 0)
                {
                    foreach (var msg in staffMessages)
                    {
                        notifyUsers(ne, messages, userMessages, "Due Loan Pmts", LOAN_PAYMENT_DUE_NOTE_TYPE_ID,
                            new List<string> { msg.Key }, staffMessages.Keys);
                    }
                }
            }
            catch (Exception) { }
        }

        private void GetLoanUnapprovedNotes(notificationsModel ne, coreLogic.coreLoansEntities le,
            Dictionary<string, Note> userMessages)
        {
            try
            { 
                var matured = le.loans
                    .Where(p => (p.loanStatusID <3))
                    .ToList();
                var messages = new Dictionary<string, List<string>>();
                var msgs = new List<string>();
                foreach (var mat in matured)
                {  
                    var message = String.Format(LoanUnapprovedTemplate,
                        "/ln/loans/image.aspx?cid=" + mat.client.clientID.ToString() + "&h=48&w=48",
                        ((mat.client.clientCompany != null) ? mat.client.clientCompany.companyName
                            : (mat.client.accountName != null && mat.client.accountName.Trim().Length > 4 ? mat.client.accountName :
                            mat.client.surName + ", " + mat.client.otherNames)),
                        "/ln/loans/loanChecklist.aspx?id=" + mat.loanID.ToString(),
                        "/ln/loans/approve.aspx?id=" + mat.loanID.ToString(),
                        mat.loanNo,
                        mat.amountRequested.ToString("#,##0.#0"), 
                        mat.applicationDate.ToString("dd-MMM-yyyy"));
                    if (mat.staffID != null && mat.staff == null && mat.staff.userName != null)
                    {
                        message = message.Replace("NOT DEFINED", mat.staff.surName + ", " + mat.staff.otherNames);
                        if (messages.ContainsKey(mat.staff.userName))
                        {
                            messages[mat.staff.userName].Add(message);
                        }
                        else
                        {
                            var msg = new List<string>();
                            msg.Add(message);
                            messages.Add(mat.staff.userName, msg);
                        }
                    }
                    msgs.Add(message);
                }
                notifyUsers(ne, msgs, userMessages, "Unapproved Loans", LOAN_UNAPPROVED_NOTE_TYPE_ID, null, messages.Keys);
                foreach (var msg in messages)
                {
                    if (msg.Value != null && msg.Value.Count > 0)
                    {
                        notifyUsers(ne, msg.Value, userMessages, "Unapproved Loans", LOAN_UNAPPROVED_NOTE_TYPE_ID,
                            new List<string> { msg.Key }, messages.Keys);
                    }
                }
            }
            catch (Exception) { }
        }

        private void GetLoanUndisbursedNotes(notificationsModel ne, coreLogic.coreLoansEntities le,
            Dictionary<string, Note> userMessages)
        {
            try
            {
                var matured = le.loans
                    .Where(p => (p.loanStatusID ==3 && p.finalApprovalDate != null))
                    .ToList();
                var messages = new Dictionary<string, List<string>>();
                var msgs = new List<string>();
                foreach (var mat in matured)
                {
                    var message = String.Format(LoanUndisbursedTemplate,
                        "/ln/loans/image.aspx?cid=" + mat.client.clientID.ToString() + "&h=48&w=48",
                        ((mat.client.clientCompany != null) ? mat.client.clientCompany.companyName
                            : (mat.client.accountName != null && mat.client.accountName.Trim().Length > 4 ? mat.client.accountName :
                            mat.client.surName + ", " + mat.client.otherNames)),
                        "/ln/loans/loan.aspx?id=" + mat.loanID.ToString(),
                        "/ln/cashier/disburse.aspx?id=" + mat.loanID.ToString(),
                        mat.loanNo,
                        mat.amountRequested.ToString("#,##0.#0"),
                        mat.amountApproved.ToString("#,##0.#0"),
                        mat.finalApprovalDate.Value.ToString("dd-MMM-yyyy"));
                    if (mat.staffID != null && mat.staff == null && mat.staff.userName != null)
                    {
                        message = message.Replace("NOT DEFINED", mat.staff.surName + ", " + mat.staff.otherNames);
                        if (messages.ContainsKey(mat.staff.userName))
                        {
                            messages[mat.staff.userName].Add(message);
                        }
                        else
                        {
                            var msg = new List<string>();
                            msg.Add(message);
                            messages.Add(mat.staff.userName, msg);
                        }
                    }
                    msgs.Add(message);
                }
                notifyUsers(ne, msgs, userMessages, "Undisbursed Loans", LOAN_UNDISBURSED_NOTE_TYPE_ID, null, messages.Keys);
                foreach (var msg in messages)
                {
                    if (msg.Value != null && msg.Value.Count > 0)
                    {
                        notifyUsers(ne, msg.Value, userMessages, "Undisbursed Loans", LOAN_UNDISBURSED_NOTE_TYPE_ID,
                            new List<string> { msg.Key }, messages.Keys);
                    }
                }
            }
            catch (Exception) { }
        }

        private void GetClientActivityNotes(notificationsModel ne, coreLogic.coreLoansEntities le,
            Dictionary<string, Note> userMessages)
        {
            try
            {
                var startDate = DateTime.Now.Date;
                var endDate = startDate.AddDays(1).AddSeconds(-1);
                var matured = le.clientActivityLogs
                    .Where(p=> p.nextActionDate>= startDate && p.nextActionDate <= endDate)
                    .ToList();
                var messages = new Dictionary<string, List<string>>();
                var msgs = new List<string>();
                foreach (var mat in matured)
                {
                    var message = String.Format(ClientActivityTemplate,
                        "/ln/loans/image.aspx?cid=" + mat.client.clientID.ToString() + "&h=48&w=48",
                        ((mat.client.clientCompany != null) ? mat.client.clientCompany.companyName
                            : (mat.client.accountName != null && mat.client.accountName.Trim().Length > 4 ? mat.client.accountName :
                            mat.client.surName + ", " + mat.client.otherNames)),
                        "/ln/loans/clientActivity.aspx?id=" + mat.clientActivityLogID.ToString(), 
                        mat.client.accountNumber,
                        mat.activityNotes, 
                        mat.nextActionDate.Value.ToString("dd-MMM-yyyy"));
                    if (mat.responsibleStaffID != null && mat.staff == null)
                    {
                        message = message.Replace("NOT DEFINED", mat.staff.surName + ", " + mat.staff.otherNames);
                        if (messages.ContainsKey(mat.staff.userName))
                        {
                            messages[mat.staff.userName].Add(message);
                        }
                        else
                        {
                            var msg = new List<string>();
                            msg.Add(message);
                            messages.Add(mat.staff.userName, msg);
                        }
                    }
                    msgs.Add(message);
                }
                notifyUsers(ne, msgs, userMessages, "Client Reminders", CLIENT_ACTIVITY_NOTE_TYPE_ID, null, messages.Keys);
                foreach (var msg in messages)
                {
                    if (msg.Value != null && msg.Value.Count > 0)
                    {
                        notifyUsers(ne, msg.Value, userMessages, "Client Reminders", CLIENT_ACTIVITY_NOTE_TYPE_ID,
                            new List<string> { msg.Key }, messages.Keys);
                    }
                }
            }
            catch (Exception) { }
        }

        private void GetOpenTillDaysNotes(notificationsModel ne, coreLogic.coreLoansEntities le,
            Dictionary<string, Note> userMessages)
        {
            try
            {
                var startDate = DateTime.Now.Date;
                var matured = le.cashiersTillDays
                    .Where(p => (p.tillDay < startDate)
                        && (p.open == true))
                    .ToList(); 
                var msgs = new List<string>();
                using (var ent = new coreLogic.coreSecurityEntities())
                {
                    foreach (var mat in matured)
                    {
                        var cashierName = ent.users.First(p => p.user_name == mat.cashiersTill.userName).full_name;
                        var message = String.Format(OpenTillDaysTemplate,
                            cashierName,
                            mat.tillDay.ToString("dd-MMM-yyyy"));
                        msgs.Add(message);
                    }
                    notifyUsers(ne, msgs, userMessages, "Open Cashiers' Tills", OPEN_TILL_DAYS_NOTE_TYPE_ID, null);
                }
            }
            catch (Exception) { }
        }

        private void sendMessages(Dictionary<string, Note> userMessages)
        {
            foreach (var msg in userMessages)
            { 
                if (Global.UserNoteRecords.ContainsKey(msg.Key.ToLower()))
                {
                    if ((DateTime.Now - Global.UserNoteRecords[msg.Key.ToLower()].lastNotified).TotalSeconds < 90)
                    {
                        continue;
                    }
                }
                else
                {
                    Global.UserNoteRecords.Add(msg.Key.ToLower(), new Global.UserNoteRecord
                    {
                        Username = msg.Key.ToLower(),
                        lastNotified = DateTime.Now
                    });
                }
                Global.UserNoteRecords[msg.Key.ToLower()].lastNotified = DateTime.Now;

                var message = string.Format(NotificationsContainerTemplate,
                    msg.Value.Header, msg.Value.Body);
                if (Global.htUsers_ConIds.ContainsKey(msg.Key.ToLower()))
                {
                    GlobalHost
                          .ConnectionManager
                          .GetHubContext<NotificationsHub>().Clients.Client(Global.htUsers_ConIds[msg.Key.ToLower()]).sendMessage(
                            msg.Key, message, msg.Value.NoOfMessages);
                }
            }
        }

        private void notifyUsers(notificationsModel ne, List<string> messages, Dictionary<string, Note> userMessages,
            string noteName, int notificationTypeID, List<string> targetUsers,
            IEnumerable<string> staffUsers =  null)
        {
            if (staffUsers == null) staffUsers = new string[] { };
            if (targetUsers != null && targetUsers.Count > 0)
            {
                foreach (var message in messages)
                {
                    foreach (var userName in targetUsers)
                    {
                        CopyMessage(userMessages, userName, message);
                    }
                }
            }
            else //if (targetUsers == null || targetUsers.Count <= 0)
            {
                var users = new List<string>();
                var privs = ne.notificationPrivileges.Where(p => p.notificationTypeID == notificationTypeID)
                    .ToList();
                using (var ent = new coreLogic.coreSecurityEntities())
                {
                    foreach (var priv in privs)
                    {
                        if (priv.allowAll == true)
                        {
                            foreach (var usr in ent.users.Where(p => p.is_active == true).ToList())
                            {
                                if (!users.Contains(usr.user_name))
                                {
                                    users.Add(usr.user_name);
                                }
                            }
                        }
                        else if (priv.roleName != null && priv.roleName.Trim().Length > 1)
                        {
                            foreach (var usr in ent.user_roles.Where(p => p.roles.role_name == priv.roleName).ToList())
                            {
                                if (!users.Contains(usr.users.user_name))
                                {
                                    users.Add(usr.users.user_name);
                                }
                            }
                        }
                        else if (priv.userName != null && priv.userName.Trim().Length > 2)
                        {
                            users.Add(priv.userName);
                        }
                    }
                }
                foreach (var message in messages)
                {
                    foreach (var user in users)
                    {
                        if (message.Length > 10)
                        {
                            CopyMessage(userMessages, user, message);
                        }
                    }
                }
            }
            foreach (var message in userMessages)
            {
                userMessages[message.Key].Body = string.Format(NoteBodyTemplate,
                    string.Format(NoteGroupTemplate, message.Value.Body));
                userMessages[message.Key].Header = string.Format(NoteHeaderTemplate, 
                    "$$ACTIVE$$", noteName);
            }
        }

        private void CopyMessage(Dictionary<string, Note> userMessages, string userName, string message)
        {
            if (userMessages.ContainsKey(userName))
            {
                userMessages[userName] = new Note
                {
                    Body = userMessages[userName].Body + message,
                    NoOfMessages = userMessages[userName].NoOfMessages + 1
                };
            }
            else
            {
                userMessages.Add(userName, new Note
                {
                    Body = message,
                    User = userName,
                    NoOfMessages = 1
                });
            }
        }
        private static string LoadFile(string fileName)
        {
            var contents = "";

            try
            {
                using (var file = new FileStream(fileName, FileMode.Open))
                {
                    using (var rdr = new StreamReader(file))
                    {
                        contents = rdr.ReadToEnd();
                    }
                }
            }
            catch (Exception) { }

            return contents;
        }

        private class Note
        {
            public string Header { get; set; }
            public string Body { get; set; }
            public string User { get; set; }
            public int NoOfMessages { get; set; }
        }
    }
}