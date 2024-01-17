using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace RevitAddin.Dockable.Example.Services
{
    public class DockablePaneCreatorService : IDisposable
    {
        private readonly UIControlledApplication application;
        private readonly Dictionary<DockablePaneId, FrameworkElement> paneIdFrameworkElements;
        private bool HasInitialized;

        public DockablePaneCreatorService(UIControlledApplication application)
        {
            this.application = application;
            this.paneIdFrameworkElements = new Dictionary<DockablePaneId, FrameworkElement>();
        }

        public void Initialize()
        {
            if (HasInitialized)
                return;

            HasInitialized = true;

            application.DockableFrameVisibilityChanged += Application_DockableFrameVisibilityChanged;
        }

        public void Dispose()
        {
            if (!HasInitialized)
                return;

            HasInitialized = false;

            application.DockableFrameVisibilityChanged -= Application_DockableFrameVisibilityChanged;

            this.paneIdFrameworkElements.Clear();
        }
        private void Application_DockableFrameVisibilityChanged(object sender, Autodesk.Revit.UI.Events.DockableFrameVisibilityChangedEventArgs e)
        {
            var paneId = e.PaneId;
            if (GetFrameworkElement(paneId) is UIElement element)
            {
                element.Visibility = e.DockableFrameShown ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public DockablePane Register<T>(Guid guid, T element) where T : FrameworkElement
        {
            return Register(guid, null, element, null);
        }

        public DockablePane Register<T>(Guid guid, T element, Action<DockablePaneProviderData> config) where T : FrameworkElement
        {
            return Register(guid, null, element, config);
        }

        public DockablePane Register<T>(Guid guid, string title, T element) where T : FrameworkElement
        {
            return Register(guid, title, element, null);
        }

        public DockablePane Register<T>(Guid guid, string title, T element, Action<DockablePaneProviderData> config) where T : FrameworkElement
        {
            var dpid = new DockablePaneId(guid);
            if (DockablePane.PaneIsRegistered(dpid) == false)
            {
                if (string.IsNullOrWhiteSpace(title))
                    if (element is Page page)
                        title = page.Title;

                if (string.IsNullOrWhiteSpace(title))
                    title = element.ToString();

                try
                {
                    var creator = new DockablePaneProviderCreator(element, config);
                    application.RegisterDockablePane(dpid, title, creator);
                    paneIdFrameworkElements[dpid] = element;
                }
                catch { }
            }
            return Get(guid);
        }

        public DockablePane Get(Guid guid)
        {
            var dpid = new DockablePaneId(guid);
            return Get(dpid);
        }

        public DockablePane Get(DockablePaneId dpid)
        {
            try
            {
                return application.GetDockablePane(dpid);
            }
            catch { }
            return null;
        }

        public FrameworkElement GetFrameworkElement(Guid guid)
        {
            var dpid = new DockablePaneId(guid);
            return GetFrameworkElement(dpid);
        }

        public FrameworkElement GetFrameworkElement(DockablePaneId dpid)
        {
            if (this.paneIdFrameworkElements.TryGetValue(dpid, out FrameworkElement element))
                return element;

            return null;
        }
    }
}
