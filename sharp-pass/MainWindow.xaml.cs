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
                    var passwordStorePath = GetPasswordStorePath(enforceSelectionOfNewPath);
                    var repo = new LibGit2Sharp.Repository(passwordStorePath);

                    Properties.Settings.Default.PasswordStorePath = passwordStorePath;
                    Properties.Settings.Default.Save();

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
            var passwordStorePath = Properties.Settings.Default.PasswordStorePath;

            if (passwordStorePath.Length == 0 || forceSelectionOfNewPath)
            {
                var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
                folderBrowserDialog.ShowNewFolderButton = false;
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;

                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    passwordStorePath = folderBrowserDialog.SelectedPath;
                }
                else
                {
                    if (passwordStorePath.Length == 0)
                    {
                        if (System.Windows.MessageBox.Show(
                            this,
                            "Exit application?",
                            "Password store folder selection", 
                            MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
                        {
                            Environment.Exit(0);
                        }
                    }
                }
            }

            return passwordStorePath;
        }
    }
}
