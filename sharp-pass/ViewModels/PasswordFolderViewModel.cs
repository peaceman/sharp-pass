using sharp_pass.DataModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace sharp_pass.ViewModel
{
    public class PasswordFolderViewModel : INotifyPropertyChanged
    {
        #region Properties
        public PasswordFolderViewModel ContainingFolder { get; private set; }
        public PasswordFolder PasswordFolder { get; private set; }

        public bool CanBeSelected
        {
            get { return false; }
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }

                if (_isExpanded && ContainingFolder != null)
                    ContainingFolder.IsExpanded = true;
            }
        }

        public CompositeCollection SubFoldersAndEntries
        {
            get
            {
                var subFoldersCollectionContainer = new CollectionContainer();
                var passwordEntriesCollectionContainer = new CollectionContainer();

                subFoldersCollectionContainer.Collection = SubFolders;
                passwordEntriesCollectionContainer.Collection = PasswordEntries;

                var compositeCollection = new CompositeCollection();
                compositeCollection.Add(subFoldersCollectionContainer);
                compositeCollection.Add(passwordEntriesCollectionContainer);

                return compositeCollection;
            }
        }
        #endregion // Properties

        #region PasswordFolder Properties
        public IReadOnlyCollection<PasswordFolderViewModel> SubFolders { get; private set; }
        public IReadOnlyCollection<PasswordEntryViewModel> PasswordEntries { get; private set; }
        public string Name
        {
            get { return PasswordFolder.DirectoryInformation.Name; }
        }
        #endregion // PasswordFolder Properties

        #region Constructors
        public PasswordFolderViewModel(PasswordFolder passwordFolder)
            : this(passwordFolder, null)
        {
        }

        public PasswordFolderViewModel(PasswordFolder passwordFolder, PasswordFolderViewModel containingFolder)
        {
            PasswordFolder = passwordFolder;
            ContainingFolder = containingFolder;

            SubFolders = new ReadOnlyCollection<PasswordFolderViewModel>(
                (from subFolder in passwordFolder.SubFolders
                    select new PasswordFolderViewModel(subFolder, this))
                    .ToList<PasswordFolderViewModel>());

            PasswordEntries = new ReadOnlyCollection<PasswordEntryViewModel>(
                (from passwordEntry in passwordFolder.PasswordEntries
                     select new PasswordEntryViewModel(passwordEntry, this))
                     .ToList<PasswordEntryViewModel>());
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
