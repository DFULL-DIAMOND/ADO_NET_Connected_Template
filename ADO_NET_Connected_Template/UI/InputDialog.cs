using System;
using System.Collections.Generic;
using System.Text;

namespace HW_ADO_NET_15_09.UI
{
    internal class InputDialog : Form
    {
        private readonly ComboBox _input;

        public string? Result { get; private set; }

        public InputDialog(string prompt, string[]? dropdownValues = null)
        {
            Text = "Ввод";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MinimizeBox = false;
            MaximizeBox = false;
            ClientSize = new Size(360, 110);

            var label = new Label
            {
                Text = prompt,
                Location = new Point(12, 15),
                AutoSize = true
            };

            _input = new ComboBox
            {
                Location = new Point(12, 40),
                Width = 336,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
            };

            if (dropdownValues != null && dropdownValues.Length > 0)
            {
                _input.Items.AddRange(dropdownValues);
                _input.DropDownStyle = ComboBoxStyle.DropDownList; // Запрещаем вводить текст вручную, только выбор из списка
                _input.SelectedIndex = 0; // Выбираем первый элемент по умолчанию
            }
            else
            {
                _input.DropDownStyle = ComboBoxStyle.DropDown; // Разрешаем ввод, если списка нет
            }

            var okButton = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new Point(192, 70),
                Width = 75
            };

            var cancelButton = new Button
            {
                Text = "Отмена",
                DialogResult = DialogResult.Cancel,
                Location = new Point(273, 70),
                Width = 75
            };

            Controls.Add(label);
            Controls.Add(_input);
            Controls.Add(okButton);
            Controls.Add(cancelButton);

            AcceptButton = okButton;   // Enter = OK
            CancelButton = cancelButton; // Esc = Отмена

            // Возвращаем фокус в поле при открытии
            Shown += (_, _) => _input.Focus();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
                Result = _input.Text;
            base.OnFormClosing(e);
        }
    }
}

