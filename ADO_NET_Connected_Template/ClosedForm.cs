using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HW_ADO_NET_15_09.Data;

// Пасхалка для GitHub
namespace HW_ADO_NET_15_09
{
    public partial class ClosedForm : Form
    {
        private Button _openButton = null!;
        private Label _connectionLabel = null!;
        private SqlConnection? _conn;
        private Form1? _mainForm;

        public ClosedForm()
        {
            InitializeComponent();

            BuildUi();
        }

        private void BuildUi()
        {
            Text = "Подключение";
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(400, 150);

            _openButton = new Button
            {
                Text = "Открыть соединение",
                Dock = DockStyle.Bottom,
                Height = 40
            };
            _connectionLabel = new Label
            {
                Text = "Соединение закрыто",
                ForeColor = Color.Red,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12)
            };

            Controls.Add(_connectionLabel);
            Controls.Add(_openButton);

            _openButton.Click += OnOpenClicked;
        }

        private void OnOpenClicked(object? sender, EventArgs e)
        {
            try
            {
                if (_conn is null || _conn.State != ConnectionState.Open)
                {
                    _conn?.Dispose();
                    _conn = new SqlConnection(ConnectionFactory.Build());
                    _conn.Open();
                }

                if (_mainForm is null)
                {
                    _mainForm = new Form1(this, _conn);

                    _mainForm.DisconnectRequested += OnMainFormDisconnect;
                }

                _mainForm.Show();
                Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось подключиться: {ex.Message}");
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _mainForm?.Dispose();
            _conn?.Dispose();
            base.OnFormClosing(e);
        }

        private void OnMainFormDisconnect()
        {
            if (_mainForm != null)
            {
                _mainForm.DisconnectRequested -= OnMainFormDisconnect;
                _mainForm.Dispose();
                _mainForm = null;
            }
        }
    }
}
