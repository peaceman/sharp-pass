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
using System.Security;
using GpgApi;
using sharp_pass.DataModels;
using sharp_pass.ViewModel;
using System.ComponentModel;

namespace sharp_pass
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        ObservableCollection<string> _PassFileCollection = new ObservableCollection<string>();
        LibGit2Sharp.Repository _repo;
        public PasswordStoreTreeViewModel TreeViewModel { get; set; }
        private PasswordDetailsViewModel _detailsViewModel;
        public PasswordDetailsViewModel DetailsViewModel
        {
            get { return _detailsViewModel; }
            set
            {
                if (value != _detailsViewModel)
                {
                    _detailsViewModel = value;
                    OnPropertyChanged("DetailsViewModel");
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            var passwordFolder = PasswordFolder.CreateWithDirectoryPath(@"c:\users\peaceman\pass");
            TreeViewModel = new PasswordStoreTreeViewModel(passwordFolder);
            base.DataContext = this;

            GpgInterface.ExePath = @"c:\Program Files (x86)\GNU\GnuPG\pub\gpg.exe";
        }

        public ObservableCollection<string> PassFileCollection { get { return _PassFileCollection; } }

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
                folderBrowserDialog.Description = "Select your password store folder";

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

        private void OnContentRendered(object sender, EventArgs e)
        {
            _repo = OpenPasswordRepository();
            FillTreeView();
        }

        private void FillTreeView()
        {
            var repoPath = new DirectoryInfo(_repo.Info.WorkingDirectory);
            //AddSubDirectoriesToTreeView(passFiles.Items, repoPath);
        }

        private void AddSubDirectoriesToTreeView(ItemCollection parentNode, DirectoryInfo parentDirectoryInfo)
        {
            foreach (var childDirectoryInfo in parentDirectoryInfo.EnumerateDirectories().Where(dirInfo => !dirInfo.Name.StartsWith(".git")))
            {
                var childNode = new TreeViewItem();
                parentNode.Add(childNode);
                childNode.Header = childDirectoryInfo.Name;
                AddSubDirectoriesToTreeView(childNode.Items, childDirectoryInfo);
            }

            foreach (var fileInfo in parentDirectoryInfo.EnumerateFiles("*.gpg"))
            {
                parentNode.Add(fileInfo);
            }
        }

        private void passFiles_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            FileInfo selectedFile;
            if (e.NewValue is System.IO.FileInfo)
            {
                selectedFile = (FileInfo)e.NewValue;
            }
            else
            {
                return;
            }
            
            string tmpDecryptionFileName = selectedFile.FullName + ".tmp";
            GpgDecrypt decrypt = new GpgDecrypt(selectedFile.FullName, tmpDecryptionFileName);
            GpgInterfaceResult result = decrypt.Execute();

            if (result.Status == GpgInterfaceStatus.Success)
            {
                displayText.Text = System.IO.File.ReadAllText(tmpDecryptionFileName);
                File.Delete(tmpDecryptionFileName);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.SetText(displayText.Text);
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            PasswordEntryViewModel selectedPasswordEntryViewModel;
            if (e.NewValue is PasswordEntryViewModel)
            {
                selectedPasswordEntryViewModel = (PasswordEntryViewModel)e.NewValue;
                DetailsViewModel = new PasswordDetailsViewModel(selectedPasswordEntryViewModel.PasswordEntry);
            }
        }

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