using coreNotificationsDAL.Abstractions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreNotificationsDAL.Helpers
{
    public class SmsDbHelper : IDisposable, ISmsDbHelper
    {
        private IDbConnection _connection;

        public void Dispose()
        {
            if (!(_connection == null) && _connection.State== ConnectionState.Open)
                _connection.Close();

            _connection?.Dispose();
        }

        public string GetSchema()
        {
            var schema = ConfigurationManager.AppSettings["SMS_SCHEMA"];
            return schema;
        }

        public IDbConnection CreateConnection(string connectionStrName = "")
        {
            var connectionStr = string.IsNullOrWhiteSpace(connectionStrName) ?
                ConfigurationManager.ConnectionStrings["SMS_CONNECTION_STRING"].ConnectionString :
                ConfigurationManager.ConnectionStrings[connectionStrName].ConnectionString;            
            _connection = new SqlConnection(connectionStr);
            return _connection;
        }
    }

}
