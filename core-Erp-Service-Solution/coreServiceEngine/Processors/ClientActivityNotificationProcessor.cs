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
    public class ClientActivityNotificationProcessor
    {
        private readonly IcoreLoansEntities le;
        private readonly Icore_dbEntities ent;
        private readonly int activityId;


        public ClientActivityNotificationProcessor(int activityId, IcoreLoansEntities lent, Icore_dbEntities ent)
        {
            this.le = lent;
            this.ent = ent;
            this.activityId = activityId;
        }

        public ClientActivityNotificationProcessor(int activityId)
        {
            this.le = new coreLoansEntities();
            this.ent = new core_dbEntities();
            this.activityId = activityId;
        }

        public void Process()
        {
            //retrieve  company details
            var pro = ent.comp_prof.FirstOrDefault();

            //check for due activities that are due
            var activity = le.clientActivityLogs
                .FirstOrDefault(p => p.clientActivityLogID == activityId);

            string fromAdd = System.Configuration.ConfigurationManager.AppSettings["NOTIFICATIONS_EMAIL"];
            string fromAddPass = System.Configuration.ConfigurationManager.AppSettings["NOTIFICATIONS_EMAIL_PASS"];

            //If its not null, create and email and send
            if (activity != null && pro != null && fromAdd != null && fromAddPass != null)
            {
                StringBuilder emailBody = new StringBuilder();
                const string senderName = "CORE ERP";
                const string emailSubject = "Notification for Client Activity";
                string toAddress = pro.email;

                
                var client = le.clients.FirstOrDefault(p => p.clientID == activity.clientID);
                var clientName = client.surName + ", " + client.otherNames;
                //build email body with repayment details
                emailBody.Append("DATE: " + DateTime.Today.ToString("dd-MMM-yyy") + "\n");
                emailBody.Append("From: " + senderName + "\n");
                emailBody.Append("Subject: " + emailSubject + "\n");
                emailBody.Append("DETAILS: \n");
                emailBody.Append(clientName + "has the activity below due. \n" + activity.activityNotes + "\n");
                emailBody.Append("Thank you. ");
                //create the email object
                var email = new EmailViewModel
                {
                    senderName = senderName,
                    subject = emailSubject,
                    fromAddress = fromAdd,
                    fromAddressPassword = fromAddPass,
                    toAddress = toAddress,
                    body = emailBody.ToString()
                };
                //send email
                var emailSender = new EmailSender();
                emailSender.SendMail(email);
                activity.notificationSent = true;
                activity.notificationSentDate = DateTime.Now;
            }
        }        
    }

    

}
