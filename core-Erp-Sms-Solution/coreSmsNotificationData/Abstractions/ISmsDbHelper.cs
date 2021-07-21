using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace coreSmsNotificationData.Abstractions
{
    public interface ISmsDbHelper
    {
        IDbConnection CreateConnection(string connectionStrName = "");
        string GetSchema();
    }
}
