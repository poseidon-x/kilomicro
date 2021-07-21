using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using coreLogic;
using coreLogic.HelperClasses;
using coreLogic.Models.Email;
using coreReports;

namespace coreService.Processors
{
    public class DepositMaturityNotificationProcessor
    {
        private readonly IcoreLoansEntities le;
        private readonly List<int> depositIds;
        private DateTime start;
        private DateTime end;

        public DepositMaturityNotificationProcessor(List<int> depositIds, IcoreLoansEntities lent, DateTime start, DateTime end)
        {
            this.le = lent;
            this.depositIds = depositIds;
            this.start = start;
            this.end = end;
        }

        public void Process()
        {

            var depositsDueForMaturity = le.deposits
                .Include(p => p.client)
                .Where(p => depositIds.Contains(p.depositID))
                .ToList();

            string fromAdd = System.Configuration.ConfigurationManager.AppSettings["INVST_MAT_EMAIL_FROM"];
            string fromAddPass = System.Configuration.ConfigurationManager.AppSettings["INVST_MAT_EMAIL_FROM_PASS"];
            string toAddresses = ConfigurationManager.AppSettings["INVST_MAT_EMAIL_TO_ADDRESSES"];
            string smtpHost = ConfigurationManager.AppSettings["SMTP_HOST"];

            

            //If its not null, create and email and send
            if (depositsDueForMaturity.Any() &&  fromAdd != null && fromAddPass != null && toAddresses != null)
            {
                StringBuilder emailBody = new StringBuilder();
                emailBody.Append("--------------------CORE ERP NOTIFICATIONS-------------------- <br /> <br />");
                emailBody.Append("INVESTMENTS DUE FOR MATURITY " + start.ToString("dd-MMM-yyy")+ " --- "+ end.ToString("dd-MMM-yyy")+ "<br /> <br />");

                //const string emailSubject = "Investment ";
                MailMessage msg = new MailMessage
                {
                    From = new MailAddress(fromAdd)
                };

                
                string[] singleToAddresses = toAddresses.Split(';');

                foreach (var singleAddress in singleToAddresses)
                {
                    msg.To.Add(singleAddress);
                }

                foreach (var deposit in depositsDueForMaturity)
                {
                    var cl = deposit.client;
                    emailBody.Append(cl.surName+ ", " +cl.otherNames +"'s Investment No. "+deposit.depositNo+" will mature on "+ 
                        deposit.maturityDate.Value.Date.ToString("dd-MMM-yyy") + "<br /> <br />");
                    deposit.maturityNotificationSent = true;
                }
                emailBody.Append("<br /> Thank you. ");

                //create the email object
                var email = new EmailWithCopyViewModel
                {
                    fromAddress = fromAdd,
                    fromAddressPassword = fromAddPass,
                    smtpHost = smtpHost
                };

                //send email
                var emailSender = new EmailSender();
                msg.Subject = "Investment Maturity Notification";
                msg.Body = emailBody.ToString();
                msg.IsBodyHtml = true;
                emailSender.SendMailWithCopy(email, msg);


                

                
            }
        }

    }

    

}
