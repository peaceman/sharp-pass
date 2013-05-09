using sharp_pass.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace sharp_pass.ViewModel
{
    public class PasswordStoreTreeViewModel
    {
        public PasswordFolderViewModel RootPasswordFolder { get; private set; }
        public CompositeCollection RootLevel 
        {
            get
            {
                return RootPasswordFolder.SubFoldersAndEntries;
            }
        }

        public PasswordStoreTreeViewModel(PasswordFolder passwordFolder)
        {
            RootPasswordFolder = new PasswordFolderViewModel(passwordFolder);
        }
    }
}
