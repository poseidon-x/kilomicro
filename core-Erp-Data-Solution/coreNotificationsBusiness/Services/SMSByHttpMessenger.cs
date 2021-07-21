using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using coreNotificationsDAL;
using System.Collections.Specialized;
using System.Net.Http;
using Newtonsoft.Json;

namespace coreNotificationsLibrary.Services
{
    public class SMSByHttpMessenger : IMessenger
    {
        public const int LENDZEE_SMS_API_CONFIG_ID = 3;
        public bool sendMessage(string recipient, string messageBody)
        {
            bool result = false;

            try
            {
                var isMsgPosted = SendGetMessage(recipient, messageBody).Result;
                if (isMsgPosted)
                    result = true;
            }
            catch (Exception x)
            {
                ExceptionManager.LogException(x, "SMSByHttpMessenger.sendMessage");
            }

            return result;
        }

        private string getFormatedUrl(string recipient, string messageBody)
        {
            string formattedUrl = "";

            using (var ne = new coreNotificationsDAL.notificationsModel())
            {
                var config = ne.messagingConfigs.FirstOrDefault();
                if (config != null)
                {
                    messageBody = (messageBody.Length > config.maxMessageLength)
                        ? messageBody.Substring(0, config.maxMessageLength)
                        : messageBody;
                    formattedUrl = config.httpMessagingUrl
                        .Replace("$$SENDER$$", config.messagingSender)
                        .Replace("$$USERNAME$$", config.httpMessagingUserName)
                        .Replace("$$PASSWORD$$", config.httpMessagingPassword)
                        .Replace("$$PHONE_NUMBER$$", getPhoneNumberInternationalized(recipient))
                        .Replace("$$MESSAGE_BODY$$", messageBody);
                }
            }

            return formattedUrl;
        }

        private string getPhoneNumberInternationalized(string inPhoneNumber)
        {
            string phoneNumber = inPhoneNumber;

            if (phoneNumber.Length == 10)
            {
                phoneNumber = "233" + phoneNumber.Substring(1);
            }
            else if (phoneNumber.Length == 9)
            {
                phoneNumber = "233" + phoneNumber;
            }

            return phoneNumber;
        }

        //POST MESSAGE

        public bool SendPostMessage(string msisdn, string messageBody)
        {
            bool sentResult = false;
            try
            {
                var notEntity = new notificationsModel();
                var config = notEntity.messagingConfigs.FirstOrDefault(r => r.messagingConfigID == LENDZEE_SMS_API_CONFIG_ID);
                if (config != null)
                {
                    messageBody = (messageBody.Length > config.maxMessageLength)
                        ? messageBody.Substring(0, config.maxMessageLength)
                        : messageBody;
                    var msgBody = WebUtility.UrlEncode(messageBody);
                    var phoneNumber = getPhoneNumberInternationalized(msisdn);
                    ServicePointManager.ServerCertificateValidationCallback += (o, cert, chain, val) => true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    var postContent = new NameValueCollection
                        {
                            {"clientId" ,config.httpMessagingUserName},
                            {"authKey" , config.httpMessagingPassword},
                            {"msisdn" , phoneNumber},
                            {"message" , msgBody},
                            {"senderId" , config.messagingSender}
                        };

                    using (WebClient webClient = new WebClient())
                    {
                        byte[] response = webClient.UploadValues(config.httpMessagingUrl, postContent);
                        if (response != null && response.Length > 1)
                        {
                            sentResult = true;
                        }
                    }

                }
                return sentResult;
            }
            catch (Exception e)
            {
                ExceptionManager.LogException(e, nameof(SendPostMessage));
                return sentResult;
            }

        }

        //GET SENDING
        public async Task<bool> SendGetMessage(string msisdn, string messageBody)
        {
            bool sentResult = false;
            try
            {
                notificationsModel notEntity = new notificationsModel();

                var config = notEntity.messagingConfigs.SingleOrDefault(r => r.messagingConfigID == LENDZEE_SMS_API_CONFIG_ID);
                if (config != null)
                {
                    //Check for message length
                    //messageBody = (messageBody.Length > config.maxMessageLength) ? messageBody.Substring(0, config.maxMessageLength) : messageBody;

                    var msgBody = WebUtility.UrlEncode(messageBody);
                    var phoneNumber = getPhoneNumberInternationalized(msisdn);
                    ServicePointManager.ServerCertificateValidationCallback += (o, cert, chain, val) => true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    var formattedUrl = config.httpMessagingUrl
                        .Replace("$$USER$$", config.httpMessagingUserName)
                        .Replace("$$PASSWORD$$", config.httpMessagingPassword)
                        .Replace("$$SENDER$$", config.messagingSender)
                        .Replace("$$MSISDN$$", phoneNumber)
                        .Replace("$$MESSAGE$$", msgBody);
                    using (WebClient webClient = new WebClient())
                    {
                        string response = await webClient.DownloadStringTaskAsync(formattedUrl);
                        if (!string.IsNullOrWhiteSpace(response) && response.Length > 1)
                        {
                            var smsResult = JsonConvert.DeserializeObject<SMSResult>(response);
                            if (smsResult != null && string.IsNullOrWhiteSpace(smsResult.error))
                                sentResult = true;
                        }
                    }

                }
                return sentResult;
            }
            catch (Exception e)
            {

                ExceptionManager.LogException(e, nameof(SendGetMessage));
                return sentResult;
            }

        }
    }

    public class SMSResult
    {
        public int responseCode { get; set; }
        public string error { get; set; }
        public string message { get; set; }
        public string status { get; set; }
    }

    public enum SMSStatusCodes
    {
        Successful=200,
        InvalidCredential= 102
    }
}
