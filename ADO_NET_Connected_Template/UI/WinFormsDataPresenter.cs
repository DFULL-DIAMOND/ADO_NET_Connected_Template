using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HW_ADO_NET_15_09.UI
{
    internal class WinFormsDataPresenter : IDataPresenter
    {
        private readonly DataGridView _grid;

        public WinFormsDataPresenter(DataGridView grid)
        {
            _grid = grid;
        }

        public void BeginResult(SqlDataReader reader)
        {
            _grid.Rows.Clear();
            _grid.Columns.Clear();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                _grid.Columns.Add(
                    columnName: reader.GetName(i),
                    headerText: reader.GetName(i));
            }
        }

        public void AppendRow(object[] values)
        {
            _grid.Rows.Add(values);
        }

        public void EndResult()
        {
            _grid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        public void ShowError(string message)
            => MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

        public void ShowMessage(string message)
            => MessageBox.Show(message, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
