﻿using Autodesk.Revit.UI;
using System;
using System.Windows;

namespace RevitAddin.Dockable.Example.Services
{
    internal class DockablePaneProviderCreator : IDockablePaneProvider
    {
        private readonly IFrameworkElementCreator frameworkElementCreator;
        private readonly IDockablePaneProvider dockablePaneProvider;

        public DockablePaneProviderCreator(IFrameworkElementCreator frameworkElementCreator)
        {
            this.frameworkElementCreator = frameworkElementCreator;
        }

        public DockablePaneProviderCreator(IFrameworkElementCreator frameworkElementCreator, IDockablePaneProvider dockablePaneProvider) : this(frameworkElementCreator)
        {
            this.dockablePaneProvider = dockablePaneProvider;
        }
        public DockablePaneProviderCreator(FrameworkElement frameworkElement)
        {
            this.frameworkElementCreator = new FrameworkElementCreator(frameworkElement);
        }

        public DockablePaneProviderCreator(FrameworkElement frameworkElement, IDockablePaneProvider dockablePaneProvider) : this(frameworkElement)
        {
            this.dockablePaneProvider = dockablePaneProvider;
        }

        public void SetupDockablePane(DockablePaneProviderData data)
        {
            dockablePaneProvider?.SetupDockablePane(data);
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
