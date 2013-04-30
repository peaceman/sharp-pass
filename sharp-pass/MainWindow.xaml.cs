using System;
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
        ObservableCollection<FooBar> _FooBarCollection = new ObservableCollection<FooBar>();

        public MainWindow()
        {
            _FooBarCollection.Add(new FooBar { Foo = "ololadin", Bar = "craplord" });
            _FooBarCollection.Add(new FooBar { Foo = ";lasdflkjaasdfasdfasddfasdfsd;lfj", Bar = "lol" });

            InitializeComponent();
        }

        public ObservableCollection<FooBar> FooBarCollection { get { return _FooBarCollection; } }

        private void AddRow(object sender, RoutedEventArgs e)
        {
            _FooBarCollection.Add(FooBarCollection.First());
        }
    }

    public class FooBar
    {
        public string Foo { get; set; }
        public string Bar { get; set; }
    }
}
