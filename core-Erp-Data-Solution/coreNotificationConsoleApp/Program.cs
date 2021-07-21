using coreNotificationConsoleApp.Helpers;
using coreNotificationsDAL;
using System;

namespace coreNotificationConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            var smsQueueHelper = new SmsQueueHelper();

            var messageEvent=new messageEvent{
                accountID=2345,
                clientID=1234,
                eventDate=DateTime.Now,
                messageBody="Test message queue from console app",
                messageEventCategoryID=8,
                eventID=3321,
                phoneNumber="0247218146",
                sender="TEST"
            };

            var isQueued = smsQueueHelper.QueueSms(messageEvent);

            if (isQueued)
                Console.WriteLine("Message queued successfully");
            else
                Console.WriteLine("Failed to queue Message");

            Console.ReadKey();

        }
    }
}
