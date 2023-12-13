using System;
using System.Windows;
using System.Windows.Controls;

namespace RevitAddin.Dockable.Example.Views
{
    public partial class PageView : Window
    {
        public PageView(Page page)
        {
            InitializeComponent();
            InitializeWindow();
            this.Frame.Content = page;
        }

        #region InitializeWindow
        private void InitializeWindow()
        {
            MinWidth = 320;
            MinHeight = 320;
            Width = MinWidth;
            Height = MinHeight;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            new System.Windows.Interop.WindowInteropHelper(this) { Owner = Autodesk.Windows.ComponentManager.ApplicationWindow };
        }
        #endregion
    }
}