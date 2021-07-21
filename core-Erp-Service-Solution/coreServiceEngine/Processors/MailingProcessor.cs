using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO; 
using Google.GData.AccessControl;
using Google.GData.Calendar;
using Google.GData.Client;
using Google.GData.Extensions;

namespace coreService
{
    class MailingProcessor
    {
        private static Uri calendarUri;//;
        private static string appName = "coreERP";
        private static string userName;
        private static string password;
        private static string senderEmail;
        private CalendarService service = null;

        public MailingProcessor()
        {
            calendarUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["calendarUri"]);
            userName = System.Configuration.ConfigurationManager.AppSettings["senderUsername"];
            password = System.Configuration.ConfigurationManager.AppSettings["senderPassword"];
            senderEmail = System.Configuration.ConfigurationManager.AppSettings["senderEmail"];
        }

        public void sendMail(string email, DateTime startTime, DateTime endTime, string employeeName, string detailRows,
            string detailRows2, string detailRows3, string detailRows4)
        {
            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.To.Add(email);
                //message.Bcc.Add("keamparbeng@mtn.com.gh");
                string msg = ReadTemplate("mailTemplate.html").Replace("$$DETAILS$$", detailRows).Replace("$$STARTTIME$$", startTime.ToString("HH:mm")
                    ).Replace("$$ENDTIME$$", endTime.ToString("HH:mm")).Replace("$$DAY$$", endTime.ToString("dddd, dd MMMM, yyyy")).Replace("$$EMPLOYEENAME$$", employeeName
                    ).Replace("$$DETAILS2$$", detailRows2).Replace("$$DETAILS3$$", detailRows3).Replace("$$DETAILS4$$", detailRows4);
                var subject = "Operations Notification @ " + endTime.ToString("dddd, dd MMMM, yyyy");
                SendCDOMessage(System.Configuration.ConfigurationManager.AppSettings["emailSender"], 
                   senderEmail, email, msg, subject);

            }
            catch (Exception ex)
            {
                ExceptionManager.LogException(ex,"MailingProcessor.sendMail");
            }

        }

        public void sendMail(string email, DateTime startTime, DateTime endTime, string employeeName, string detailRows
            , string assignedEmp)
        {
            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.To.Add(email);
                //message.Bcc.Add("keamparbeng@mtn.com.gh");
                string msg = ReadTemplate("mailTemplateDue.html").Replace("$$DETAILS$$", detailRows).Replace("$$STARTTIME$$", startTime.ToString("HH:mm")
                    ).Replace("$$ENDTIME$$", endTime.ToString("HH:mm")).Replace("$$DAY$$", endTime.ToString("dddd, dd MMMM, yyyy")).Replace("$$EMPLOYEENAME$$", employeeName
                    ).Replace("$$ASSIGNEDEMPLOYEE$$",assignedEmp);
                var subject = assignedEmp + "'s Due Assigned Clients @ " + endTime.ToString("dddd, dd MMMM, yyyy");
                SendCDOMessage("kofi@acsghana.com", "\"Link Exchange\"<le@acsghana.com>", email, msg, subject);

            }
            catch (Exception ex)
            {
                ExceptionManager.LogException(ex, "MailingProcessor.sendMail");
            }

        }

        public void sendMail3(string email, string employeeName, string detailRows
            , string assignedEmp)
        {
            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.To.Add(email);
                //message.Bcc.Add("keamparbeng@mtn.com.gh");
                string msg = ReadTemplate("mailTemplateTR.html").Replace("$$DETAILS$$", detailRows
                    ).Replace("$$EMPLOYEENAME$$", employeeName
                    ).Replace("$$STAFFNAME$$", assignedEmp);
                var subject = assignedEmp + "'s Upcoming Activities for Today";
                SendCDOMessage("kofi@acsghana.com", senderEmail, email, msg, subject);

            }
            catch (Exception ex)
            {
                ExceptionManager.LogException(ex, "MailingProcessor.sendMail");
            }

        }

        public void sendMail2(string email, DateTime startTime, DateTime endTime, string employeeName, string detailRows
            , string detailRows2)
        {
            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.To.Add(email);
                //message.Bcc.Add("keamparbeng@mtn.com.gh");
                string msg = ReadTemplate("mailTemplateTx.html").Replace("$$DETAILS$$", detailRows).Replace("$$STARTTIME$$", startTime.ToString("HH:mm")
                    ).Replace("$$ENDTIME$$", endTime.ToString("HH:mm")).Replace("$$DAY$$", endTime.ToString("dddd, dd MMMM, yyyy")).Replace("$$EMPLOYEENAME$$", employeeName
                    ).Replace("$$DETAILS2$$", detailRows2);
                var subject = "Transactions Occured on Link Exchange @ " + endTime.ToString("HH:mm, dd MMMM, yyyy");
                SendCDOMessage(userName, senderEmail, email, msg, subject);

            }
            catch (Exception ex)
            {
                ExceptionManager.LogException(ex, "MailingProcessor.sendMail");
            }

        }

        private void SendCDOMessage(string userName, string fromAddress, string toAddress,string body, string subject)
        {
            try
            { 
                CDO.Message message = new CDO.Message();
                CDO.IConfiguration configuration = message.Configuration;
                ADODB.Fields fields = configuration.Fields;
                 
                // Set configuration.
                // sendusing:               cdoSendUsingPort, value 2, for sending the message using the network.
                // smtpauthenticate:     Specifies the mechanism used when authenticating to an SMTP service over the network.
                //                                  Possible values are:
                //                                  - cdoAnonymous, value 0. Do not authenticate.
                //                                  - cdoBasic, value 1. Use basic clear-text authentication. (Hint: This requires the use of "sendusername" and "sendpassword" fields)
                //                                  - cdoNTLM, value 2. The current process security context is used to authenticate with the service.

                ADODB.Field field = fields["http://schemas.microsoft.com/cdo/configuration/smtpserver"];
                field.Value = "smtp.gmail.com";

                field = fields["http://schemas.microsoft.com/cdo/configuration/smtpserverport"];
                field.Value = 465;

                field = fields["http://schemas.microsoft.com/cdo/configuration/sendusing"];
                field.Value = CDO.CdoSendUsing.cdoSendUsingPort;

                field = fields["http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"];
                field.Value = CDO.CdoProtocolsAuthentication.cdoBasic;

                field = fields["http://schemas.microsoft.com/cdo/configuration/sendusername"];
                field.Value = userName;

                field = fields["http://schemas.microsoft.com/cdo/configuration/sendpassword"];
                field.Value = password;

                field = fields["http://schemas.microsoft.com/cdo/configuration/smtpusessl"];
                field.Value = "true";

                fields.Update();
                 
                message.From = fromAddress;
                message.To = toAddress;
                message.Subject = subject;
                message.HTMLBody = body;
                 
                // Send message.
                message.Send();

            }
            catch (Exception ex)
            {
                ExceptionManager.LogException(ex, "SendCDOMessage");
            }
        }

        private string ReadTemplate(string file)
        {
            try
            {
                FileStream s = new FileStream(System.Windows.Forms.Application.StartupPath+ @"\" + file, FileMode.Open);
                StreamReader r = new StreamReader(s);
                string str = r.ReadToEnd();
                r.Close();
                s.Close();

                return str;
            }
            catch (Exception x)
            {
                return "";
            }
        }

        private string ReadTemplate2()
        {
            try
            {
                FileStream s = new FileStream(System.Windows.Forms.Application.StartupPath + @"\mailTemplate2.html", FileMode.Open);
                StreamReader r = new StreamReader(s);
                string str = r.ReadToEnd();
                r.Close();
                s.Close();

                return str;
            }
            catch (Exception x)
            {
                return "";
            }
        }

        public void CreateCalendarEntry(string title, string description, string location, DateTime start, DateTime endTime,
            string attendee)
        {
            if (service == null)
            {
                service = new CalendarService(appName);
                service.setUserCredentials(userName, password);
            }
            EventEntry entry = new EventEntry();

            entry.Title.Text = XmlUtils.ToString(title);

            string htmlDescription = XmlUtils.ToString(description);
            if (htmlDescription != null && htmlDescription.Length > 0)
            {
                entry.Content.Type = "html";
                entry.Content.Content = htmlDescription;
            }
            if (location != null && location.Length > 0)
            {
                Where eventLocation = new Where();
                eventLocation.ValueString = location;
                entry.Locations.Add(eventLocation);
            } 

            When eventTime = new When();
            eventTime.StartTime = start;              
            eventTime.EndTime = endTime;
            Reminder reminder = new Reminder();
            reminder.Minutes = 15;
            reminder.Method = Reminder.ReminderMethod.alert | Reminder.ReminderMethod.sms;
            eventTime.Reminders.Add(reminder);
            entry.Times.Add(eventTime);

            Who participant = new Who();
            participant.Email = attendee;
            participant.Rel = Who.RelType.EVENT_ATTENDEE;
            entry.Participants.Add(participant);

            AtomPerson author = new AtomPerson(AtomPersonType.Author);
            author.Name = "coreERP Microfinance Solution by ACS";
            author.Email = userName;
            entry.Authors.Add(author);
            AtomEntry insertedEntry = service.Insert(calendarUri, entry);
        }
    }
}
