using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace HW_ADO_NET_15_09.Data
{
    internal class QueryDefinition
    {
        public required string Sql { get; init; }
        public string[] Prompts { get; init; } = Array.Empty<string>();
        public string? DropdownSql { get; init; }
        public bool IsAction { get; init; } = false;
        public Func<string[], SqlParameter[]> ParametersFactory { get; init; }
            = _ => Array.Empty<SqlParameter>();
    }
}
