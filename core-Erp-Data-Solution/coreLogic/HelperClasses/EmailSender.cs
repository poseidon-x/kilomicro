using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using coreLogic.Models.Email;
using coreLogic.Models.Loans;
using System.Net;
using System.Net.Mail;

namespace coreLogic.HelperClasses
{
    public class EmailSender
    {
        public void SendMail(EmailViewModel email)
        {
            var smtp = new System.Net.Mail.SmtpClient();
            {
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(email.fromAddress, email.fromAddressPassword);
                smtp.Timeout = 100000;
            }
            smtp.Send(email.fromAddress, email.toAddress, email.subject, email.body);
        }

        public void SendMailWithCopy(EmailWithCopyViewModel emailDetail, MailMessage msg)
        {
            var smtp = new SmtpClient();
            {
                smtp.Host = emailDetail.smtpHost;
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(emailDetail.fromAddress, emailDetail.fromAddressPassword);
                smtp.Timeout = 100000;
            }
            smtp.Send(msg);
        }
    }
}
