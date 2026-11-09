using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace HW_ADO_NET_15_09.Data
{
    internal static class ConnectionFactory
    {
        public static string Build()
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = "(localdb)\\MSSQLLocalDB",
                InitialCatalog = "MyDatabase",
                IntegratedSecurity = true
            };
            return builder.ConnectionString;
        }
    }
}
