using GpgApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharp_pass.DataModels
{
    public class PasswordEntry
    {
        public FileInfo FileInformation { get; private set; }

        public PasswordEntry(FileInfo fileInformation)
        {
            FileInformation = fileInformation;
        }

        public String Password
        {
            get
            {
                string tmpDecryptionFileName = FileInformation.FullName + ".tmp";
                GpgDecrypt decrypt = new GpgDecrypt(FileInformation.FullName, tmpDecryptionFileName);
                GpgInterfaceResult result = decrypt.Execute();

                string password = System.IO.File.ReadAllText(tmpDecryptionFileName).Trim();
                File.Delete(tmpDecryptionFileName);

                return password;
            }
        }
    }
}
