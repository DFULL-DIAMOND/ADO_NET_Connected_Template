using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HW_ADO_NET_15_09.UI
{
    public interface IDataPresenter
    {
        void BeginResult(SqlDataReader reader);
        void AppendRow(object[] values);
        void EndResult();
        void ShowError(string message);
        void ShowMessage(string message);
    }

    public interface IUserPrompt
    {
        string[]? Ask(string[] prompts, string[]? dropdownValues = null);
    }
}
