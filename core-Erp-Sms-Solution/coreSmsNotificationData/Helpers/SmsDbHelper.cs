using coreSmsNotificationData.Abstractions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace coreSmsNotificationData.Helpers
{
    public class SmsDbHelper : IDisposable, ISmsDbHelper
    {
        private IDbConnection _connection;

        private IConfiguration _configuration;

        public SmsDbHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Dispose()
        {
            if (!(_connection is null) && _connection.State is ConnectionState.Open)
                _connection.Close();

            _connection?.Dispose();
        }

        public string GetSchema()
        {
            var schema = _configuration.GetConnectionString("SmsSchema");
            return schema;
        }

        public IDbConnection CreateConnection(string connectionStrName = "")
        {
            var connectionStr = string.IsNullOrWhiteSpace(connectionStrName) ?
                _configuration.GetConnectionString("SmsDatabase") :
                _configuration.GetConnectionString(connectionStrName);
            _connection = new SqlConnection(connectionStr);
            return _connection;
        }
    }
}
