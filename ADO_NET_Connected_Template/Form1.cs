using Microsoft.Data.SqlClient;
using System.Data;
using HW_ADO_NET_15_09.Data;
using HW_ADO_NET_15_09.Queries;
using HW_ADO_NET_15_09.UI;

namespace HW_ADO_NET_15_09
{
    public partial class Form1 : Form
    {
        public event Action? DisconnectRequested;

        private readonly QueryRunner _runner;
        private readonly ClosedForm _parent;

        private SqlConnection _conn;
        private Label _actionLabel = null!;
        private DataGridView _grid = null!;
        private ComboBox _queryPicker = null!;
        private Button _runButton = null!;
        private Label _connectionLabel = null!;

        public Form1(ClosedForm parent, SqlConnection conn)
        {
            _parent = parent;
            _conn = conn;
            InitializeComponent();
            BuildUi();
            _runner = new QueryRunner(new WinFormsDataPresenter(_grid));
        }

        private void BuildUi()
        {
            Text = "Фирма канцтоваров";
            Width = 900;
            Height = 600;
            MinimumSize = new Size(600, 400);
            StartPosition = FormStartPosition.CenterScreen;

            _connectionLabel = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 24,
                Text = "Соединение закрыто",
                ForeColor = Color.Red
            };
            _connectionLabel.Text = _conn.State == ConnectionState.Open
                ? "Соединение открыто"
                : "Соединение закрыто";
            _connectionLabel.ForeColor = _conn.State == ConnectionState.Open
                ? Color.Green
                : Color.Red;

            _actionLabel = new Label
            {
                Text = "Выберите действие:",
                Dock = DockStyle.Top
            };

            _queryPicker = new ComboBox
            {
                Dock = DockStyle.Top,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _queryPicker.Items.AddRange(new object[]
            {
                "1.  Ваш тестовый запрос",
                "0.  Закрыть соединение и выйти"
            });
            _queryPicker.SelectedIndex = 0;

            _runButton = new Button { Text = "Выполнить", Dock = DockStyle.Top };
            _runButton.Click += OnRunClicked;

            _grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            };

            FormClosing += OnFormClosing;

            // Порядок добавления важен: Fill добавляется первым, Top — последними
            Controls.Add(_grid);
            Controls.Add(_runButton);
            Controls.Add(_queryPicker);
            Controls.Add(_actionLabel);
            Controls.Add(_connectionLabel);
        }

        private void OnFormClosing(object? sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
        }
        private async void OnRunClicked(object? sender, EventArgs e)
        {
            string key = _queryPicker.Text.Split('.')[0].Trim();

            if (key == "0")
            {
                _conn.Close();

                DisconnectRequested?.Invoke();

                _parent.Show();
                Hide();
                return;
            }

            if (!QueryRegistry.TryGet(key, out var query))
                return;

            if (_conn.State != ConnectionState.Open)
            {
                MessageBox.Show("Соединение закрыто");
                return;
            }

            try
            {
                // 1. Получаем данные для выпадающего списка (если он нужен)
                string[]? dropdownValues = null;
                if (!string.IsNullOrEmpty(query.DropdownSql))
                {
                    var list = new List<string>();
                    using var cmd = new SqlCommand(query.DropdownSql, _conn);
                    using var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        list.Add(reader.GetString(0)); // Берем первую колонку
                    }
                    dropdownValues = list.ToArray();
                }

                // 2. Вызываем окно ввода, передавая ему список
                var prompt = new WinFormsUserPrompt();
                var inputs = prompt.Ask(query.Prompts, dropdownValues);

                if (inputs is null)   // пользователь отменил ввод
                    return;

                var parameters = query.ParametersFactory(inputs);

                await _runner.ExecuteAsync(_conn, query, parameters);

            }   // ← conn.Dispose() автоматически
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
