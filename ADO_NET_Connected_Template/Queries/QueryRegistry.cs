using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
using HW_ADO_NET_15_09.Data;

namespace HW_ADO_NET_15_09.Queries
{
    internal class QueryRegistry
    {
        private static readonly Dictionary<string, QueryDefinition> _queries = new()
        {
            ["1"] = new() { Sql = "SELECT * FROM dbo.YourDataTable"}
        };

        public static bool TryGet(string key, out QueryDefinition query)
            => _queries.TryGetValue(key, out query!);
    }
}
