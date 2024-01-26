using Autodesk.Revit.UI;
using System;
using System.Windows;

namespace RevitAddin.Dockable.Example.Services
{
    internal class DockablePaneProviderCreator : IDockablePaneProvider
    {
        private readonly FrameworkElement frameworkElement;
        private readonly IDockablePaneProvider dockablePaneProvider;

        public DockablePaneProviderCreator(FrameworkElement frameworkElement)
        {
            this.frameworkElement = frameworkElement;
        }

        public DockablePaneProviderCreator(FrameworkElement frameworkElement, IDockablePaneProvider dockablePaneProvider) : this(frameworkElement)
        {
            this.dockablePaneProvider = dockablePaneProvider;
        }

        public void SetupDockablePane(DockablePaneProviderData data)
        {
            dockablePaneProvider?.SetupDockablePane(data);
            data.FrameworkElement = frameworkElement;
        }
    }
}
