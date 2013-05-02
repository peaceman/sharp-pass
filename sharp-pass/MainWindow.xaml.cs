using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace sharp_pass
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<string> _PassFileCollection = new ObservableCollection<string>();

        public MainWindow()
        {
            var passwordRepo = OpenPasswordRepository();

            foreach (var filePath in Directory.EnumerateFiles(GetPasswordStorePath(), "*.gpg", SearchOption.AllDirectories))
            {
                _PassFileCollection.Add(filePath);
            }

            InitializeComponent();
        }

        public ObservableCollection<string> PassFileCollection { get { return _PassFileCollection; } }

        private void didSelectItem(object sender, SelectionChangedEventArgs e)
        {

        }

        protected LibGit2Sharp.Repository OpenPasswordRepository()
        {
            var enforceSelectionOfNewPath = false;

            while (true)
            {
                try
                {
                    var repo = new LibGit2Sharp.Repository(GetPasswordStorePath(enforceSelectionOfNewPath));
                    return repo;
                }
                catch
                {
                    enforceSelectionOfNewPath = true;
                }
            }
        }

        protected string GetPasswordStorePath(bool forceSelectionOfNewPath = false)
        {
            if (Properties.Settings.Default.PasswordStorePath.Length == 0 || forceSelectionOfNewPath)
            {
                var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
                folderBrowserDialog.ShowNewFolderButton = false;
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;

                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Properties.Settings.Default.PasswordStorePath = folderBrowserDialog.SelectedPath;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    System.Environment.Exit(0);
                }
            }

            return Properties.Settings.Default.PasswordStorePath;
        }
    }
}
