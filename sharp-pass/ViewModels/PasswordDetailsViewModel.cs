using sharp_pass.Commands;
using sharp_pass.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharp_pass.ViewModel
{
    public class PasswordDetailsViewModel : INotifyPropertyChanged
    {
        public static string PASSWORD_STARS = "****";

        #region Properties
        public PasswordEntry PasswordEntry { get; private set; }

        private string _displayedPassword = PASSWORD_STARS;
        public string DisplayedPassword
        {
            get { return _displayedPassword; }
            set
            {
                if (value != _displayedPassword)
                {
                    _displayedPassword = value;
                    OnPropertyChanged("DisplayedPassword");
                }
            }
        }

        private bool _isDisplayingClearTextPassword;
        public bool IsDisplayingClearTextPassword
        {
            get { return _isDisplayingClearTextPassword; }
            set
            {
                if (value != _isDisplayingClearTextPassword)
                {
                    _isDisplayingClearTextPassword = value;
                    OnPropertyChanged("IsDisplayingClearTextPassword");

                    if (!_isDisplayingClearTextPassword)
                        DisplayedPassword = PASSWORD_STARS;
                    else
                        DisplayedPassword = PasswordEntry.Password;
                }
            }
        }

        public PasswordEntryCopyToClipboardCommand CopyToClipboardCommand { get; private set; }
        #endregion // Properties

        #region Constructors
        public PasswordDetailsViewModel(PasswordEntry passwordEntry)
        {
            PasswordEntry = passwordEntry;
            CopyToClipboardCommand = new PasswordEntryCopyToClipboardCommand(passwordEntry);
        }
        #endregion // Constructors

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion // INotifyPropertyChanged Members
    }
}
