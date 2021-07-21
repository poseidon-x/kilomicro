using coreSmsNotificationData.Abstractions;
using coreSmsNotificationData.Helpers;
using coreSmsNotificationData.Model;
using Dapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreSmsNotificationData.Repositories
{
    public class SmsDataRepository : ISmsDataRepository
    {
        private readonly ISmsDbHelper _smsDbHelper;
        private readonly ILogger<ISmsDataRepository> _logger;
        private const string SmsConnectionString = "SmsDatabase";
        public SmsDataRepository(ISmsDbHelper smsDbHelper, 
            ILogger<ISmsDataRepository> logger)
        {
            _smsDbHelper = smsDbHelper;
            _logger = logger;
        }

        public bool QueueSmsEvent(messageEvent input)
        {
            try
            {
                using (var connection = _smsDbHelper.CreateConnection(SmsConnectionString))
                {
                    var queueResult = connection.ExecuteScalar<int>($"{_smsDbHelper.GetSchema()}.queue_message_event",
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
                _logger.LogError(e, "An error occurred processing {MethodName} with {Input}",
                    nameof(QueueSmsEvent), JsonConvert.SerializeObject(input));
                return false;
            }
        }


        public async Task<List<messageEvent>> GetAllQueuedUnsentSmsEvents()
        {
            try
            {
                using var connection = _smsDbHelper.CreateConnection(SmsConnectionString);
                var queueResult = await connection.QueryAsync<messageEvent>($"{_smsDbHelper.GetSchema()}.get_all_unsent_message_events",
                    commandType: CommandType.StoredProcedure
                    );
                if (queueResult != null)
                    return queueResult.ToList();
                return null;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred processing {MethodName}",
                    nameof(GetAllQueuedUnsentSmsEvents));
                return null;
            }
        }


        public string GetDisbursementSmsTemplate()
        {
            try
            {
                using var connection = _smsDbHelper.CreateConnection(SmsConnectionString);
                var tempResult = connection.QuerySingleOrDefault<string>($"{_smsDbHelper.GetSchema()}.get_disbursement_sms_template",

                    commandType: CommandType.StoredProcedure
                    );
                if (!string.IsNullOrWhiteSpace(tempResult))
                    return tempResult;
                return null;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred processing {MethodName}",
                    nameof(GetDisbursementSmsTemplate));
                return null;
            }
        }


        public bool MarkSmsEventAsFinished(int messageEventId)
        {
            try
            {
                using (var connection = _smsDbHelper.CreateConnection(SmsConnectionString))
                {
                    var queueResult = connection.ExecuteScalar<int>($"{_smsDbHelper.GetSchema()}.mark_sms_as_sent",
                        new
                        {
                            MessageEventId= messageEventId
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
                _logger.LogError(e, "An error occurred processing {MethodName} with {Input}",
                    nameof(MarkSmsEventAsFinished),new { messageEventId });
                return false;
            }
        }


        public messagingConfig GetMessageConfig(int configId)
        {
            try
            {
                using var connection = _smsDbHelper.CreateConnection(SmsConnectionString);
                var msgConfResult = connection.QuerySingleOrDefault<messagingConfig>($"{_smsDbHelper.GetSchema()}.get_sms_config",
                    new {
                        MessageConfigId=configId
                    },

                    commandType: CommandType.StoredProcedure
                    );
                
                return msgConfResult;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred processing {MethodName} with {Input}",
                    nameof(GetMessageConfig),new { configId });
                return null;
            }
        }

    }
}
