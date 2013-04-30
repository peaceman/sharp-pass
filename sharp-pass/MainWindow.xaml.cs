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
            var pathToPassFiles = @"c:\users\peaceman\pass";
            var repo = new LibGit2Sharp.Repository(pathToPassFiles);

            foreach (var filePath in Directory.EnumerateFiles(pathToPassFiles, "*.gpg", SearchOption.AllDirectories))
            {
                _PassFileCollection.Add(filePath);
            }

            InitializeComponent();
        }

        public ObservableCollection<string> PassFileCollection { get { return _PassFileCollection; } }
    }
}
