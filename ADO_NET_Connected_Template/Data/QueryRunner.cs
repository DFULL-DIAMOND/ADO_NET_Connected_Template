using HW_ADO_NET_15_09.UI;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HW_ADO_NET_15_09.Data
{
    internal class QueryRunner
    {
        private readonly IDataPresenter _presenter;

        public QueryRunner(IDataPresenter presenter)
        {
            _presenter = presenter;
        }

        public async Task ExecuteAsync(SqlConnection conn, QueryDefinition query, SqlParameter[] parameters)
        {
            using var cmd = new SqlCommand(query.Sql, conn);
            if (parameters.Length > 0)
                cmd.Parameters.AddRange(parameters);

            if (query.IsAction)
            {
                int rows = await cmd.ExecuteNonQueryAsync();
                _presenter.ShowMessage($"Успешно! Затронуто строк: {rows}");
                return;
            }

            using var reader = await cmd.ExecuteReaderAsync();

            // 1. Сообщаем presenter'у схему — он создаёт колонки
            _presenter.BeginResult(reader);

            // 2. Построчно читаем и отдаём значения
            while (await reader.ReadAsync())
            {
                var values = new object[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                    values[i] = await reader.IsDBNullAsync(i) ? DBNull.Value : reader.GetValue(i);

                _presenter.AppendRow(values);
            }

            // 3. Завершаем вывод (например, авто-ширина колонок)
            _presenter.EndResult();
        }
    }
}