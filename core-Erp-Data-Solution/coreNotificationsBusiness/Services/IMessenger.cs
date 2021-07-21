using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreNotificationsLibrary.Services
{
    public interface IMessenger
    {
        bool sendMessage(string recipient, string messageBody);
    }
}
