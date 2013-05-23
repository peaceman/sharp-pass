using sharp_pass.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sharp_pass.Commands
{
    public class PasswordEntryCopyToClipboardCommand : ICommand
    {
        public PasswordEntry PasswordEntry { get; private set; }

        public PasswordEntryCopyToClipboardCommand(PasswordEntry passwordEntry)
        {
            PasswordEntry = passwordEntry;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            System.Windows.Clipboard.SetText(PasswordEntry.Password);
        }
    }
}
