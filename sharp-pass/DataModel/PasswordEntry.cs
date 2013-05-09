using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharp_pass.DataModel
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
                return "dummy password";
            }
        }
    }
}
