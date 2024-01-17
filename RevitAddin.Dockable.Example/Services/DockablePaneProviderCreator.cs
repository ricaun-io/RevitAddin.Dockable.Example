using Autodesk.Revit.UI;
using System;
using System.Windows;

namespace RevitAddin.Dockable.Example.Services
{
    public class DockablePaneProviderCreator : IDockablePaneProvider
    {
        private readonly IFrameworkElementCreator frameworkElementCreator;
        private readonly Action<DockablePaneProviderData> config;

        public DockablePaneProviderCreator(IFrameworkElementCreator frameworkElementCreator)
        {
            this.frameworkElementCreator = frameworkElementCreator;
        }

        public DockablePaneProviderCreator(IFrameworkElementCreator frameworkElementCreator, Action<DockablePaneProviderData> config) : this(frameworkElementCreator)
        {
            this.config = config;
        }

        public DockablePaneProviderCreator(FrameworkElement frameworkElement)
        {
            this.frameworkElementCreator = new FrameworkElementCreator(frameworkElement);
        }

        public DockablePaneProviderCreator(FrameworkElement frameworkElement, Action<DockablePaneProviderData> config) : this(frameworkElement)
        {
            this.config = config;
        }

        public void SetupDockablePane(DockablePaneProviderData data)
        {
            config?.Invoke(data);
            data.FrameworkElementCreator = frameworkElementCreator;
            data.FrameworkElement = null;
        }

        class FrameworkElementCreator : IFrameworkElementCreator
        {
            private readonly FrameworkElement frameworkElement;

            public FrameworkElementCreator(FrameworkElement frameworkElement)
            {
                this.frameworkElement = frameworkElement;
            }
            public FrameworkElement CreateFrameworkElement()
            {
                return frameworkElement;
            }
        }
    }
}
