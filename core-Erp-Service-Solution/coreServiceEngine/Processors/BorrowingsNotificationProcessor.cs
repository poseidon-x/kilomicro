using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;
using coreLogic.HelperClasses;
using coreLogic.Models.Email;
using coreReports;

namespace coreService.Processors
{
    public class BorrowingsNotificationProcessor
    {
        private readonly IcoreLoansEntities le;
        private readonly Icore_dbEntities ent;
        private readonly int borrowingId;
        private readonly DateTime currentDate;

        //variable to indicate when to send email(In this case 3 days to repayment date)
        private const int NUMBER_OF_DAYS_TO_REPAYMENT_DATE = -3;
        
        public BorrowingsNotificationProcessor(int borrowingId, IcoreLoansEntities lent, Icore_dbEntities ent,
            DateTime currentDate)
        {
            this.le = lent;
            this.ent = ent;
            this.borrowingId = borrowingId;
            this.currentDate = currentDate;
        }

        public void Process()
        {
            //retrieve  company details
            var pro = ent.comp_prof.FirstOrDefault();

            //retrieve borrowings to send mails for.
            var brw = le.borrowings
                .Where(p => p.amountDisbursed > 0 && p.disbursedDate != null && p.balance > 0 && !p.closed)
                .Include(p => p.borrowingRepaymentSchedules)
                .FirstOrDefault(p => p.borrowingId == borrowingId);

            //check for repayment schedule that is ... days to due
            var repayment = brw.borrowingRepaymentSchedules
                .FirstOrDefault(p => p.repaymentDate > DateTime.Now 
                && p.repaymentDate.AddDays(NUMBER_OF_DAYS_TO_REPAYMENT_DATE).Date == DateTime.Today);

            //If its not null, create and email and send
            if (repayment != null)
            {
                StringBuilder emailBody = new StringBuilder();
                const string senderName = "CORE ERP";
                const string emailSubject = "Notification for Due Borrowing Repayment";
                const string fromAddress = "man@acsghana.com";
                const string fromAddressPass = "202051ifokook";
                string toAddress = pro.email;

                //build email body with repayment details
                emailBody.Append("DATE: " + currentDate + "\n");
                emailBody.Append("From: " + senderName + "\n");
                emailBody.Append("Subject: " + emailSubject + "\n");
                emailBody.Append("CONTENT: \n");
                emailBody.Append("Borrowing Account Number: " + brw.borrowingNo + " has a scheduled payment due on"
                    + repayment.repaymentDate.ToString("dddd d-MMMM-yyyy") + "\n");
                emailBody.Append("DETAILS: \n");
                emailBody.Append("Principal Payment: " + repayment.principalPayment.ToString("N") + "\n");
                emailBody.Append("Interest Payment: " + repayment.interestPayment.ToString("N") + "\n");

                //create the email object
                var email = new EmailViewModel
                {
                    senderName = senderName,
                    subject = emailSubject,
                    fromAddress = fromAddress,
                    fromAddressPassword = fromAddressPass,
                    toAddress = toAddress,
                    body = emailBody.ToString()
                };

                //send email
                var emailSender = new EmailSender();
                emailSender.SendMail(email);
            }
        }        
    }

    

}
