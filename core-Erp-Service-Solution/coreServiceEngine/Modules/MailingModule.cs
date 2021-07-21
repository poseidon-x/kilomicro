using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Data;

namespace coreService
{
    class MailingModule
    {
        public bool StopFlag{get;set;}
        public bool Stopped { get; set;}

        public void Main()
        {
            List<int> repaymentIDs = new List<int>();
            List<int> tranchIDs = new List<int>();
            while (StopFlag == false && System.Configuration.ConfigurationManager.AppSettings["emailEnabled"] == "Y")
            {
                var morningStartHour = int.Parse(System.Configuration.ConfigurationManager.AppSettings["morningStartHour"]);
                var afterNoonStartHour = int.Parse(System.Configuration.ConfigurationManager.AppSettings["afterNoonStartHour"]);
                var eveningStartHour = int.Parse(System.Configuration.ConfigurationManager.AppSettings["eveningStartHour"]);
                var startMin = int.Parse(System.Configuration.ConfigurationManager.AppSettings["startMin"]);
                var startSec = int.Parse(System.Configuration.ConfigurationManager.AppSettings["startSec"]);
                var calendarStartHour = int.Parse(System.Configuration.ConfigurationManager.AppSettings["calendarStartHour"]);
                try
                {
                    var yesterDay = DateTime.Now.AddDays(-1);
                    if (yesterDay.DayOfWeek == DayOfWeek.Sunday)
                    {
                        yesterDay = yesterDay.AddDays(-1);
                    }
                    var startDate = DateTime.Now;
                    var endDate = DateTime.Now;
                    if (DateTime.Now.Hour == morningStartHour)
                    {
                        startDate = new DateTime(yesterDay.Year, yesterDay.Month, yesterDay.Day, 7, 0, 0);
                        endDate = new DateTime(yesterDay.Year, yesterDay.Month, yesterDay.Day, 20, 0, 0);
                    }
                    else if (DateTime.Now.Hour == afterNoonStartHour)
                    {
                        startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
                        endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 0);
                    }
                    else if (DateTime.Now.Hour == eveningStartHour)
                    {
                        startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
                        endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 16, 0, 0);
                    }
                    coreLogic.coreLoansEntities le = new coreLogic.coreLoansEntities();
//                    var proc = new MailingProcessor();
//                    if (((DateTime.Now.Hour == morningStartHour && DateTime.Now.Minute == startMin && DateTime.Now.Second == startSec )
//                        || (DateTime.Now.Hour == afterNoonStartHour && DateTime.Now.Minute == startMin && DateTime.Now.Second == startSec )
//                        || (DateTime.Now.Hour == eveningStartHour && DateTime.Now.Minute == startMin && DateTime.Now.Second == startSec )
//                        )
//                        && (DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
//                        )
//                    {
//                        repaymentIDs.Clear();
//                        tranchIDs.Clear();
//                        var disb = (
//                                from d in le.loanTranches
//                                from l in le.loans
//                                from c in le.clients
//                                where d.loanID == l.loanID
//                                    && l.clientID == c.clientID
//                                    && d.creation_date >= startDate && d.creation_date <= endDate
//                                select new
//                                {
//                                    clientName = c.surName + ", " + c.otherNames,
//                                    c.accountNumber,
//                                    l.loanNo,
//                                    amount = d.amountDisbursed
//                                }
//                            ).ToList();

//                        var rcpt = (
//                                from d in le.loanRepayments
//                                from l in le.loans
//                                from c in le.clients
//                                where d.loanID == l.loanID
//                                    && l.clientID == c.clientID
//                                    && d.creation_date >= startDate && d.creation_date <= endDate
//                                select new
//                                {
//                                    clientName = c.surName + ", " + c.otherNames,
//                                    c.accountNumber,
//                                    l.loanNo,
//                                    amount = d.amountPaid
//                                }
//                            ).ToList();

//                        var und = (
//                                from l in le.loans
//                                from c in le.clients
//                                where l.clientID == c.clientID &&
//                                    l.loanStatusID == 3
//                                select new
//                                {
//                                    clientName = c.surName + ", " + c.otherNames,
//                                    c.accountNumber,
//                                    l.loanNo,
//                                    amount = l.amountApproved
//                                }
//                            ).ToList();

//                        var una = (
//                                from l in le.loans
//                                from c in le.clients
//                                where l.clientID == c.clientID &&
//                                    l.loanStatusID < 3
//                                select new
//                                {
//                                    clientName = c.surName + ", " + c.otherNames,
//                                    c.accountNumber,
//                                    l.loanNo,
//                                    amount = l.amountRequested
//                                }
//                            ).ToList();

//                        var r2 =
//                            (from n in le.notifications
//                             from r1 in le.notificationRecipients
//                             from s in le.staffs
//                             where n.notificationID == r1.notificationID
//                                && r1.staffID == s.staffID
//                               && n.notificationCode == "N1"
//                             select new
//                             {
//                                 r1.email,
//                                 staffName = s.otherNames + " " + s.surName
//                             }).ToList();
//                        ;

//                        foreach (var r in r2)
//                        {
//                            string details = "";
//                            string details2 = "";
//                            string details3 = "";
//                            string details4 = "";
//                            double td = 0.0, tr = 0.0, tu = 0.0, ta = 0.0;
//                            foreach (var d in disb)
//                            {

//                                details += @"<tr>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>" + d.accountNumber + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;font-weight:bold !important;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-left-style:solid;border-left-width:1px !important;border-left-color:#DFDFDF !important;font-size:12px;'>" + d.clientName + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>" + d.loanNo + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;text-align:right;'>" + d.amount.ToString("#,##0.#0") + @" </td>
//                                   </tr>";
//                                td += d.amount;
//                            }
//                            details += @"<tr>
//                                <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>&nbsp;</td>
//                                <td style='background-color:white !important;border-bottom-style:solid;font-weight:bold !important;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-left-style:solid;border-left-width:1px !important;border-left-color:#DFDFDF !important;font-size:12px;'>&nbsp;</td>
//                                <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>Total</td>
//                                <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;text-align:right;'>" + td.ToString("#,##0.#0") + @" </td>
//                                </tr>";
//                            foreach (var d in rcpt)
//                            {

//                                details2 += @"<tr>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>" + d.accountNumber + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;font-weight:bold !important;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-left-style:solid;border-left-width:1px !important;border-left-color:#DFDFDF !important;font-size:12px;'>" + d.clientName + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>" + d.loanNo + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;text-align:right;'>" + d.amount.ToString("#,##0.#0") + @" </td>
//                                   </tr>";
//                                tr += d.amount;
//                            }
//                            details2 += @"<tr>
//                                <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>&nbsp;</td>
//                                <td style='background-color:white !important;border-bottom-style:solid;font-weight:bold !important;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-left-style:solid;border-left-width:1px !important;border-left-color:#DFDFDF !important;font-size:12px;'>&nbsp;</td>
//                                <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>Total</td>
//                                <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;text-align:right;'>" + tr.ToString("#,##0.#0") + @" </td>
//                                </tr>";
//                            foreach (var d in una)
//                            {

//                                details3 += @"<tr>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>" + d.accountNumber + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;font-weight:bold !important;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-left-style:solid;border-left-width:1px !important;border-left-color:#DFDFDF !important;font-size:12px;'>" + d.clientName + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>" + d.loanNo + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;text-align:right;'>" + d.amount.ToString("#,##0.#0") + @" </td>
//                                   </tr>";
//                                tu += d.amount;
//                            }
//                            details3 += @"<tr>
//                                <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>&nbsp;</td>
//                                <td style='background-color:white !important;border-bottom-style:solid;font-weight:bold !important;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-left-style:solid;border-left-width:1px !important;border-left-color:#DFDFDF !important;font-size:12px;'>&nbsp;</td>
//                                <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>Total</td>
//                                <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;text-align:right;'>" + tu.ToString("#,##0.#0") + @" </td>
//                                </tr>";
//                            foreach (var d in und)
//                            {

//                                details4 += @"<tr>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>" + d.accountNumber + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;font-weight:bold !important;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-left-style:solid;border-left-width:1px !important;border-left-color:#DFDFDF !important;font-size:12px;'>" + d.clientName + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>" + d.loanNo + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;text-align:right;'>" + d.amount.ToString("#,##0.#0") + @" </td>
//                                   </tr>";
//                                ta += d.amount;
//                            }
//                            details4 += @"<tr>
//                                <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>&nbsp;</td>
//                                <td style='background-color:white !important;border-bottom-style:solid;font-weight:bold !important;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-left-style:solid;border-left-width:1px !important;border-left-color:#DFDFDF !important;font-size:12px;'>&nbsp;</td>
//                                <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>Total</td>
//                                <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;text-align:right;'>" + ta.ToString("#,##0.#0") + @" </td>
//                                </tr>";

//                            if (td > 0 || tr > 0 || tu > 0 || ta > 0)
//                            {
//                                proc.sendMail(r.email, startDate, endDate,
//                                   r.staffName, details, details2, details3, details4);
//                            }
//                        }
//                    }
//                    if (((DateTime.Now.Hour == morningStartHour && DateTime.Now.Minute == startMin && DateTime.Now.Second == startSec))
//                        && (DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
//                        )
//                    {
//                        var r3 =
//                            (from n in le.notifications
//                             from r1 in le.notificationRecipients
//                             from s in le.staffs
//                             where n.notificationID == r1.notificationID
//                                && r1.staffID == s.staffID
//                               && n.notificationCode == "N2"
//                             select new
//                             {
//                                 r1.email,
//                                 r1.staffID,
//                                 staffName = s.otherNames + " " + s.surName
//                             }).ToList();
//                        ;

//                        foreach (var r in r3)
//                        {
//                            var staffs = (from s in le.staffs
//                                          from l in le.loans
//                                          where s.staffID == l.staffID
//                                             && l.balance > 0
//                                          group l by new { s.surName, s.otherNames, s.staffID } into g
//                                          select new
//                                          {
//                                              g.Key.staffID,
//                                              g.Key.surName,
//                                              g.Key.otherNames
//                                          }).Distinct().ToList();
//                            foreach (var s in staffs)
//                            {
//                                var due =
//                                    (from rs in le.repaymentSchedules
//                                     from l in le.loans
//                                     from c in le.clients
//                                     where rs.loanID == l.loanID
//                                        && l.clientID == c.clientID
//                                        && l.staffID == s.staffID
//                                        && rs.principalBalance + rs.interestBalance > 2
//                                        && l.balance > 0
//                                        && rs.repaymentDate <= DateTime.Now
//                                     group rs by new { c.surName, c.otherNames, c.accountNumber, l.loanNo, rs.loanID } into g
//                                     select new
//                                     {
//                                         clientName = g.Key.surName + ", " + g.Key.otherNames,
//                                         g.Key.accountNumber,
//                                         g.Key.loanNo,
//                                         amount = g.Sum(t => t.principalBalance + t.interestBalance),
//                                         date = g.Max(t => t.repaymentDate)
//                                     }
//                                         ).OrderByDescending(p => p.date).ToList();
//                                string details = "";
//                                double td = 0.0;
//                                foreach (var d in due)
//                                {

//                                    details += @"<tr>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>" + d.accountNumber + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;font-weight:bold !important;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-left-style:solid;border-left-width:1px !important;border-left-color:#DFDFDF !important;font-size:12px;'>" + d.clientName + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>" + d.loanNo + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>" + d.date.ToString("dd-MMM-yyyy") + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;text-align:right;'>" + d.amount.ToString("#,##0.#0") + @" </td>
//                                   </tr>";
//                                    td += d.amount;
//                                }
//                                details += @"<tr>
//                                <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>&nbsp;</td>
//                                <td style='background-color:white !important;border-bottom-style:solid;font-weight:bold !important;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-left-style:solid;border-left-width:1px !important;border-left-color:#DFDFDF !important;font-size:12px;'>&nbsp;</td>
//                                <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>&nbsp;</td>
//                                <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>Total</td>
//                                <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;text-align:right;'>" + td.ToString("#,##0.#0") + @" </td>
//                                </tr>";
//                                if (td > 0)
//                                {
//                                    proc.sendMail(r.email, startDate, endDate,
//                                       r.staffName, details, s.otherNames + " " + s.surName);
//                                }
//                            }
//                        }
//                    }
//                    if (((DateTime.Now.Second == startSec && DateTime.Now.Hour == calendarStartHour && DateTime.Now.Minute == startMin)))
//                    {
//                        var r3 =
//                            (from n in le.notifications
//                             from r1 in le.notificationRecipients
//                             from s in le.staffs
//                             where n.notificationID == r1.notificationID
//                                && r1.staffID == s.staffID
//                               && n.notificationCode == "N4"
//                             select new
//                             {
//                                 r1.email,
//                                 r1.staffID,
//                                 staffName = s.otherNames + " " + s.surName
//                             }).ToList();
//                        ;

//                        var sd = DateTime.Now.Date.AddDays(-1); 
//                        var ed = sd.AddHours(23.99); 
//                        var log =
//                            (from rs in le.clientActivityLogs
//                             from c in le.clients
//                             from t in le.clientActivityTypes
//                             where rs.clientID == c.clientID
//                             && rs.clientActivityTypeID == t.clientActivityTypeID
//                             && rs.creationDate <= DateTime.Now && rs.creationDate >= sd
//                             && rs.nextActionDate != null
//                             select new
//                             {
//                                 clientName = c.surName + ", " + c.otherNames,
//                                 c.accountNumber,
//                                 rs.activityDate,
//                                 rs.activityNotes,
//                                 rs.clientActivityLogID,
//                                 t.clientActivityTypeName,
//                                 rs.creationDate,
//                                 rs.creator,
//                                 rs.nextAction,
//                                 rs.nextActionDate,
//                                 rs.responsibleStaffID
//                             }
//                                ).ToList();
//                        foreach (var r in r3)
//                        {
//                            string details = "";
//                            foreach (var d in log)
//                            {
//                                if (r.staffID == d.responsibleStaffID)
//                                {
//                                    try
//                                    {
//                                        details = "<b>Client:</b> " + d.clientName + "<br />" +
//                                            "<b>Category:</b> " + d.clientActivityTypeName + "<br />" +
//                                            "<b>Activity:</b> " + d.activityNotes + "<br />" +
//                                            "<b>Next Action:</b> " + d.nextAction + "<br />";
//                                        proc.CreateCalendarEntry("Task Reminder: " + d.clientName,
//                                            details, d.clientActivityTypeName, d.nextActionDate.Value.Date.AddHours(8.5), d.nextActionDate.Value.Date.AddHours(17),
//                                            r.email);
//                                    }
//                                    catch (Exception x)
//                                    {
//                                        ExceptionManager.LogException(x, "MailingModule.Main");
//                                    }
//                                }
//                            }
//                        }
//                    }
//                    if (((DateTime.Now.Second == startSec && DateTime.Now.Hour == calendarStartHour && DateTime.Now.Minute == startMin+2)))
//                    {
//                        var r3 =
//                            (from n in le.notifications
//                             from r1 in le.notificationRecipients
//                             from s in le.staffs
//                             where n.notificationID == r1.notificationID
//                                && r1.staffID == s.staffID
//                               && n.notificationCode == "N5"
//                             select new
//                             {
//                                 r1.email,
//                                 r1.staffID,
//                                 staffName = s.otherNames + " " + s.surName
//                             }).ToList();
//                        ;


//                        var sd = DateTime.Now.Date;
//                        var ed = sd.AddHours(23.99); 
//                        var log =
//                            (from rs in le.clientActivityLogs
//                             from c in le.clients
//                             from t in le.clientActivityTypes
//                             from s in le.staffs
//                             where rs.clientID == c.clientID
//                             && rs.clientActivityTypeID == t.clientActivityTypeID
//                             && rs.responsibleStaffID==s.staffID
//                             && (rs.nextActionDate <= ed && rs.nextActionDate >= sd)
//                             select new
//                             {
//                                 clientName = c.surName + ", " + c.otherNames,
//                                 c.accountNumber,
//                                 rs.activityDate,
//                                 rs.activityNotes,
//                                 rs.clientActivityLogID,
//                                 t.clientActivityTypeName,
//                                 rs.creationDate,
//                                 rs.creator,
//                                 nextAction=(rs.nextAction==null)?rs.activityNotes:rs.nextAction,
//                                 nextActionDate = rs.nextActionDate,
//                                 rs.responsibleStaffID,
//                                 staffName = s.surName + ", " + s.otherNames,
//                             }
//                                ).ToList();
//                        foreach (var r in r3)
//                        {
//                            string details = "";
//                            string staffName = "";
//                            foreach (var d in log)
//                            {
//                                details += @"<tr>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>" + d.accountNumber + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;font-weight:bold !important;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-left-style:solid;border-left-width:1px !important;border-left-color:#DFDFDF !important;font-size:12px;'>" + d.clientName + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>" + d.nextAction + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>" + d.nextActionDate.Value.ToString("dd-MMM-yyyy HH:mm") + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;text-align:right;'>" + d.activityNotes + @" </td>
//                                   </tr>";
//                                staffName = d.staffName;
//                            } 
                             
//                            if (details!= "")
//                            {
//                                proc.sendMail3(r.email ,
//                                   r.staffName, details, staffName);
//                            }
//                        } 
//                    }
//                    if (((DateTime.Now.Second == startSec)))
//                    {
//                        var r3 =
//                            (from n in le.notifications
//                             from r1 in le.notificationRecipients
//                             from s in le.staffs
//                             where n.notificationID == r1.notificationID
//                                && r1.staffID == s.staffID
//                               && n.notificationCode == "N3"
//                             select new
//                             {
//                                 r1.email,
//                                 r1.staffID,
//                                 staffName = s.otherNames + " " + s.surName
//                             }).ToList();
//                        ;

//                        var sd = DateTime.Now.AddMinutes(-5);
//                        var disb =
//                            (from rs in le.loanTranches
//                             from l in le.loans
//                             from c in le.clients
//                             where rs.loanID == l.loanID
//                             && l.clientID == c.clientID
//                             && rs.creation_date <= DateTime.Now && rs.creation_date >= sd
//                             select new
//                             {
//                                 clientName = c.surName + ", " + c.otherNames,
//                                 c.accountNumber,
//                                 l.loanNo,
//                                 amount = rs.amountDisbursed,
//                                 date = rs.disbursementDate,
//                                 rs.loanTranchID
//                             }
//                                ).OrderByDescending(p => p.date).ToList();
//                        var rcpt =
//                            (from rs in le.loanRepayments
//                             from l in le.loans
//                             from c in le.clients
//                             where rs.loanID == l.loanID
//                             && l.clientID == c.clientID
//                             && rs.creation_date <= DateTime.Now && rs.creation_date >= sd
//                             select new
//                             {
//                                 clientName = c.surName + ", " + c.otherNames,
//                                 c.accountNumber,
//                                 l.loanNo,
//                                 amount = rs.amountPaid,
//                                 date = rs.repaymentDate,
//                                 rs.loanRepaymentID
//                             }
//                                ).OrderByDescending(p => p.date).ToList();
//                        foreach (var r in r3)
//                        {
//                            string details = "";
//                            string details2 = "";
//                            double td = 0.0, tr = 0.0;
//                            foreach (var d in disb)
//                            {
//                                if (tranchIDs.Contains(d.loanTranchID) == false)
//                                {
//                                    details += @"<tr>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>" + d.accountNumber + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;font-weight:bold !important;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-left-style:solid;border-left-width:1px !important;border-left-color:#DFDFDF !important;font-size:12px;'>" + d.clientName + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>" + d.loanNo + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>" + d.date.ToString("dd-MMM-yyyy HH:mm") + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;text-align:right;'>" + d.amount.ToString("#,##0.#0") + @" </td>
//                                   </tr>";
//                                    td += d.amount;
//                                }
//                            }
//                            details += @"<tr>
//                                <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>&nbsp;</td>
//                                <td style='background-color:white !important;border-bottom-style:solid;font-weight:bold !important;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-left-style:solid;border-left-width:1px !important;border-left-color:#DFDFDF !important;font-size:12px;'>&nbsp;</td>
//                                <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>&nbsp;</td>
//                                <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>Total Disbursed</td>
//                                <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;text-align:right;'>" + td.ToString("#,##0.#0") + @" </td>
//                                </tr>";

//                            foreach (var d in rcpt)
//                            {
//                                if (repaymentIDs.Contains(d.loanRepaymentID) == false)
//                                {
//                                    details2 += @"<tr>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>" + d.accountNumber + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;font-weight:bold !important;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-left-style:solid;border-left-width:1px !important;border-left-color:#DFDFDF !important;font-size:12px;'>" + d.clientName + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>" + d.loanNo + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>" + d.date.ToString("dd-MMM-yyyy HH:mm") + @"</td>
//                                    <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;text-align:right;'>" + d.amount.ToString("#,##0.#0") + @" </td>
//                                   </tr>";
//                                    tr += d.amount;
//                                }
//                            }
//                            details2 += @"<tr>
//                                <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>&nbsp;</td>
//                                <td style='background-color:white !important;border-bottom-style:solid;font-weight:bold !important;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-left-style:solid;border-left-width:1px !important;border-left-color:#DFDFDF !important;font-size:12px;'>&nbsp;</td>
//                                <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>&nbsp;</td>
//                                <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;'>Total Repayments</td>
//                                <td style='background-color:white !important;border-bottom-style:solid;border-bottom-width:1px !important;border-bottom-color:#DFDFDF !important;border-right-style:solid;border-right-width:1px !important;border-right-color:#DFDFDF !important;font-size:12px;text-align:right;'>" + tr.ToString("#,##0.#0") + @" </td>
//                                </tr>";

//                            if (td > 0 || tr > 0)
//                            {
//                                proc.sendMail2(r.email, startDate, endDate,
//                                   r.staffName, details, details2);
//                            }
//                        }
//                        foreach (var d in disb)
//                        {
//                            tranchIDs.Add(d.loanTranchID);
//                        }
//                        foreach (var d in rcpt)
//                        {
//                            repaymentIDs.Add(d.loanRepaymentID);
//                        }
//                    }
                }
                catch (Exception x)
                {
                    ExceptionManager.LogException(x, "MailingModule.Main");
                }
                System.Threading.Thread.Sleep(100000);
                if (StopFlag == true) break;
            }
            Stopped = true;
        }
    }
}
