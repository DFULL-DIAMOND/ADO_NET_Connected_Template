using System;
using System.Collections.Generic;
using System.Text;

namespace HW_ADO_NET_15_09.UI
{
    // Текстовый запрос пользователю
    internal class WinFormsUserPrompt : IUserPrompt
    {
        public string[] Ask(string[] prompts, string[]? dropdownValues = null)
        {
            if (prompts.Length == 0)
                return Array.Empty<string>();

            var inputs = new string[prompts.Length];

            for (int i = 0; i < prompts.Length; i++)
            {
                string[]? currentDropdown = (i == 0) ? dropdownValues : null;

                using var dialog = new InputDialog(prompts[i], currentDropdown);
                var result = dialog.ShowDialog();

                if (result != DialogResult.OK)
                {
                    return null!;
                }

                inputs[i] = dialog.Result ?? string.Empty;
            }

            return inputs;
        }
    }
}
