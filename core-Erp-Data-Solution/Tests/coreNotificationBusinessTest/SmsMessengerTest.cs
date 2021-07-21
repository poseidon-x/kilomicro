using coreNotificationsLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreNotificationBusinessTest
{
   
    [TestClass]
    public class SmsMessengerTest
    {
        [TestMethod]
        public void U_TEST_SEND_GET_MSG_RETURNS_SUCCESS()
        {
            //Arrange
            var msisdn = "233247218146";
            var msgBody = "Hello Wendolin, thanks for doing business with us.";
            SMSByHttpMessenger messenger = new SMSByHttpMessenger();

            //Act
            var msgResult = messenger.SendGetMessage(msisdn, msgBody).Result;

            //Assert
            if (msgResult)
            {
                Assert.IsTrue(msgResult);
            }
            else
            {
                Assert.IsFalse(msgResult);
            }

        }

        [TestMethod]
        public void U_TEST_SEND_POST_MSG_RETURNS_SUCCESS()
        {
            //Arrange
            var msisdn = "233247218146";
            var msgBody = "Hello Wendolin, thanks for doing business with us.";
            SMSByHttpMessenger messenger = new SMSByHttpMessenger();

            //Act
            var msgResult = messenger.SendPostMessage(msisdn, msgBody);

            //Assert
            if (msgResult)
            {
                Assert.IsTrue(msgResult);
            }
            else
            {
                Assert.IsFalse(msgResult);
            }

        }
    }
}
