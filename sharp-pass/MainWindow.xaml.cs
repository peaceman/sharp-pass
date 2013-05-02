﻿using System;
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
        LibGit2Sharp.Repository _repo;

        public MainWindow()
        {
            InitializeComponent();
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
            //foreach (var filePath in Directory.EnumerateFiles(GetPasswordStorePath(), "*.gpg", SearchOption.AllDirectories))
            //{
            //    string head, tail, directory;
            //    directory = System.IO.Path.GetDirectoryName(filePath);
            //    EquivalentSplit(directory, out head, out tail);

            //    _PassFileCollection.Add(filePath);
            //}
        }

        private void FillTreeView()
        {
            var repoPath = new DirectoryInfo(_repo.Info.WorkingDirectory);
            AddSubDirectoriesToTreeView(passFiles.Items, repoPath);
        }

        private void AddSubDirectoriesToTreeView(ItemCollection parentNode, DirectoryInfo parentDirectoryInfo)
        {
            foreach (var childDirectoryInfo in parentDirectoryInfo.EnumerateDirectories())
            {
                var childNode = new TreeViewItem();
                parentNode.Add(childNode);
                childNode.Header = childDirectoryInfo.Name;
                AddSubDirectoriesToTreeView(childNode.Items, childDirectoryInfo);
            }

            foreach (var fileInfo in parentDirectoryInfo.EnumerateFiles())
            {
                parentNode.Add(fileInfo.Name);
            }
        }

        public void EquivalentSplit(string path, out string head, out string tail)
        {

            // Get the directory separation character (i.e. '\').
            string separator = System.IO.Path.DirectorySeparatorChar.ToString();

            // Trim any separators at the end of the path
            string lastCharacter = path.Substring(path.Length - 1);
            if (separator == lastCharacter)
            {
                path = path.Substring(0, path.Length - 1);
            }

            int lastSeparatorIndex = path.LastIndexOf(separator);

            head = path.Substring(0, lastSeparatorIndex);
            tail = path.Substring(lastSeparatorIndex + separator.Length,
                path.Length - lastSeparatorIndex - separator.Length);

        }
    }
}