using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreNotificationsDAL.Abstractions
{
    public interface ISmsDbHelper
    {
        IDbConnection CreateConnection(string connectionStrName = "");
        string GetSchema();
    }
}
