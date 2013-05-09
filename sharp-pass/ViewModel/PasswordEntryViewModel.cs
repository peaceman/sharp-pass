using sharp_pass.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharp_pass.ViewModel
{
    public class PasswordEntryViewModel : INotifyPropertyChanged
    {
        #region Properties
        public PasswordFolderViewModel ContainingFolder { get; private set; }
        public PasswordEntry PasswordEntry { get; private set; }

        private bool _isSelected;
        public bool IsSelected 
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
                }

                if (ContainingFolder != null)
                    ContainingFolder.IsExpanded = true;
            }
        }
        #endregion // Properties

        #region PasswordEntry Properties
        public string Name
        {
            get
            {
                return PasswordEntry.FileInformation.Name;
            }
        }
        #endregion // PasswordEntry Properties

        #region Constructors
        public PasswordEntryViewModel(PasswordEntry passwordEntry)
            : this(passwordEntry, null)
        {
        }

        public PasswordEntryViewModel(PasswordEntry passwordEntry, PasswordFolderViewModel containingFolder)
        {
            PasswordEntry = passwordEntry;
            ContainingFolder = containingFolder;
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
