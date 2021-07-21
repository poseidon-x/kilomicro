using coreNotificationsDAL.Abstractions;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreNotificationsDAL.Helpers
{
    public class SmsDataHelper : ISmsDataHelper
    {
        ISmsDbHelper smsDbHelper;
        private const string SmsConnectionString = "SMS_CONNECTION_STRING";
        public SmsDataHelper()
        {
            smsDbHelper = new SmsDbHelper();
        }

        public bool QueueSmsEvent(messageEvent input)
        {
            try
            {
                using (var connection = smsDbHelper.CreateConnection(SmsConnectionString))
                {
                    var queueResult = connection.ExecuteScalar<int>($"{smsDbHelper.GetSchema()}.queue_message_event",
                        new
                        {
                            MessageCategoryId = input.messageEventCategoryID,
                            ClientId = input.clientID,
                            AccId = input.accountID,
                            EventId = input.eventID,
                            PhoneNumber = input.phoneNumber,
                            MessageBody = input.messageBody,
                            SenderName = input.sender,
                            MessageDate = input.eventDate
                        },
                        commandType: CommandType.StoredProcedure
                        );
                    if (queueResult > 0)
                        return true;                    
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public string GetDisbursementSmsTemplate()
        {
            try
            {
                using (var connection = smsDbHelper.CreateConnection(SmsConnectionString))
                {
                    var tempResult = connection.QuerySingleOrDefault<string>($"{smsDbHelper.GetSchema()}.get_disbursement_sms_template",
                        
                        commandType: CommandType.StoredProcedure
                        );
                    if (!string.IsNullOrWhiteSpace(tempResult))
                        return tempResult;
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
