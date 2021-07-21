using coreSmsNotificationData.Abstractions;
using coreSmsNotificationData.Extensions;
using coreSmsNotificationData.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartFormat;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace coreSmsNotificationData.Helpers
{
    public class SmsHttpHelper : HttpClient, ISmsHttpHelper
    {
        private readonly ISmsDataRepository _smsDataHelper;
        private readonly ILogger<ISmsHttpHelper> _logger;
        public SmsHttpHelper()
        {

        }

        private readonly IHostEnvironment _env;
        public SmsHttpHelper(ILogger<ISmsHttpHelper> logger,
            ISmsDataRepository smsDataHelper
            )
        {
            _logger = logger;
            _smsDataHelper = smsDataHelper;
            DefaultRequestHeaders.Accept.Clear();
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Timeout = TimeSpan.FromSeconds(10000);
        }

        public async Task<SmsSentResult> SendSmsByGetMethodAsync(string msisdn, string messageBody,
            string sender, int messageConfigId)
        {
            int statusCode = 500;
            try
            {
                var msgBody = WebUtility.UrlEncode(messageBody);
                var smsConfig = _smsDataHelper.GetMessageConfig(messageConfigId);
                var senderId = string.IsNullOrWhiteSpace(sender) ?
                    smsConfig?.messagingSender :
                    sender;
                var requestUrl = smsConfig.httpMessagingUrl;

                //check for prefix international
                string phoneNumber = "";
                try
                {
                    phoneNumber = msisdn.ToIntlFormat();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    return new SmsSentResult
                    {
                        IsSent = false,
                        Error = $"You have entered an invalid phone number :- {msisdn}",
                        StatusCode = statusCode,
                    };
                }

                var formattedUriString = requestUrl
                    .Replace("$$USER$$", smsConfig.httpMessagingUserName)
                    .Replace("$$PASSWORD$$", smsConfig.httpMessagingPassword)
                    .Replace("$$SENDER$$", senderId)
                    .Replace("$$MSISDN$$", phoneNumber)
                    .Replace("$$MESSAGE$$", msgBody);

                string response = await GetStringAsync(formattedUriString);
                var smsResult = new SMSResult();
                if (!string.IsNullOrWhiteSpace(response) && response.Length > 1)
                {
                    smsResult = JsonConvert.DeserializeObject<SMSResult>(response);
                    if (smsResult != null && string.IsNullOrWhiteSpace(smsResult.error))
                        return new SmsSentResult
                        {
                            IsSent = true,
                            StatusCode = smsResult.responseCode,
                            Message = smsResult?.message
                        };
                }
                statusCode = smsResult.responseCode;
                return new SmsSentResult
                {
                    IsSent = false,
                    Error = smsResult?.error,
                    StatusCode = smsResult.responseCode,
                    Message = smsResult?.message
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred executing {MethodName} in {ClassName}-\n{Input}\n{Error}",
                    nameof(SendSmsByGetMethodAsync), nameof(SmsHttpHelper), new
                    {
                        msisdn,
                        messageBody,
                        sender,
                        messageConfigId
                    }, e);
                return new SmsSentResult
                {
                    IsSent = false,
                    Error = "An exception occurred. Please try again",
                    StatusCode = statusCode
                };
            }
        }

    }
}
