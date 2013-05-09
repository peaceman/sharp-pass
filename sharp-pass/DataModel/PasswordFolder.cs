using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharp_pass.DataModel
{
    public class PasswordFolder
    {
        public DirectoryInfo DirectoryInformation { get; private set; }
        public IEnumerable<PasswordEntry> PasswordEntries 
        {
            get
            {
                return FetchPasswordEntriesFromDirectoryInformation();
            }
        }
        public IEnumerable<PasswordFolder> SubFolders
        {
            get
            {
                return FetchSubFoldersFromDirectoryInformation();
            }
        }

        #region Constructors
        public PasswordFolder(DirectoryInfo directoryInformation)
        {
            DirectoryInformation = directoryInformation;
        }

        public static PasswordFolder CreateWithDirectoryPath(String directoryPath)
        {
            var directoryInformation = new DirectoryInfo(directoryPath);
            if (!directoryInformation.Exists)
                throw new ArgumentException("Given directory (" + directoryPath + ") does not exist!", "directoryPath");

            return new PasswordFolder(directoryInformation);
        }
        #endregion

        private IEnumerable<PasswordFolder> FetchSubFoldersFromDirectoryInformation()
        {
            foreach (var directoryInformation in DirectoryInformation.EnumerateDirectories().Where(dirInfo => !dirInfo.Name.StartsWith(".git")))
                yield return new PasswordFolder(directoryInformation);
        }

        private IEnumerable<PasswordEntry> FetchPasswordEntriesFromDirectoryInformation()
        {
            foreach (var fileInformation in DirectoryInformation.EnumerateFiles("*.gpg"))
                yield return new PasswordEntry(fileInformation);
        }
    }
}
