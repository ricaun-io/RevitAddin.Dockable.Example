using RevitAddin.Dockable.Example.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace RevitAddin.Dockable.Example.Views
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public partial class DockablePage2 : Page
    {
        public static Guid Guid => new Guid("F2F1F1F1-1F1F-1F1F-1F1F-1F1F1F1F1F1F");
        public static Guid Guid3 => new Guid("F3F1F1F1-1F1F-1F1F-1F1F-1F1F1F1F1F1F");
        public DockablePage2()
        {
            //this.Loaded += (s, e) => { Console.WriteLine($"Loaded:\t {this.GetHashCode()}"); };
            //this.Unloaded += (s, e) => { Console.WriteLine($"Unloaded:\t {this.GetHashCode()}"); };
            //this.IsVisibleChanged += (s, e) => { Console.WriteLine($"IsVisibleChanged:\t {this.GetHashCode()} \t{this.IsVisible}"); };
            //this.IsEnabledChanged += (s, e) => { Console.WriteLine($"IsEnabledChanged:\t {this.GetHashCode()} \t{this.IsEnabled}"); };

            InitializeComponent();
        }

        public int Number { get; set; }
        public void OnNumberChanged()
        {
            if (this.Number % 2 == 0)
            {
                this.Background = System.Windows.Media.Brushes.LightSalmon;
            }
            else
            {
                this.Background = System.Windows.Media.Brushes.LightSkyBlue;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            button.Content = ++Number;
        }
    }
}
