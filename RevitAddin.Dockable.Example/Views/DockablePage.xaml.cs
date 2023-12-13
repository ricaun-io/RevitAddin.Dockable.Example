using Autodesk.Revit.UI;
using System;
using System.Windows;
using System.Windows.Controls;

namespace RevitAddin.Dockable.Example.Views
{
    public partial class DockablePage : Page, IDockablePaneProvider
    {
        public static Guid Guid => new Guid("F1F1F1F1-1F1F-1F1F-1F1F-1F1F1F1F1F1F");
        public DockablePage()
        {
            InitializeComponent();
        }

        public void SetupDockablePane(DockablePaneProviderData data)
        {
            data.FrameworkElement = this;

            data.InitialState = new DockablePaneState
            {
                DockPosition = DockPosition.Tabbed,
            };
        }

        private int Number { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            button.Content = Number++;
        }
    }
}