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
        private readonly Dictionary<DockablePaneId, IDockablePaneDocumentProvider> dockablePaneDocumentProvider;
        private bool HasInitialized;

        public DockablePaneCreatorService(UIControlledApplication application)
        {
            this.application = application;
            this.paneIdFrameworkElements = new Dictionary<DockablePaneId, FrameworkElement>();
            this.dockablePaneDocumentProvider = new Dictionary<DockablePaneId, IDockablePaneDocumentProvider>();
        }

        public void Initialize()
        {
            if (HasInitialized)
                return;

            HasInitialized = true;

            application.DockableFrameVisibilityChanged += Application_DockableFrameVisibilityChanged;
            application.Idling += Application_Idling;
        }

        public void Dispose()
        {
            if (!HasInitialized)
                return;

            HasInitialized = false;

            application.DockableFrameVisibilityChanged -= Application_DockableFrameVisibilityChanged;
            application.Idling -= Application_Idling;

            this.paneIdFrameworkElements.Clear();
            this.dockablePaneDocumentProvider.Clear();
        }

        Queue<DockablePaneId> dockablePaneIds = new Queue<DockablePaneId>();
        private void Application_DockableFrameVisibilityChanged(object sender, Autodesk.Revit.UI.Events.DockableFrameVisibilityChangedEventArgs e)
        {
            var paneId = e.PaneId;
            dockablePaneIds.Enqueue(paneId);
        }

        private void ExecutePaneDocumentProviderChanged(UIApplication uiapp, DockablePaneId dockablePaneId)
        {
            if (GetFrameworkElement(dockablePaneId) is FrameworkElement element)
            {
                dockablePaneDocumentProvider.TryGetValue(dockablePaneId, out IDockablePaneDocumentProvider documentProvider);
                var document = uiapp.ActiveUIDocument?.Document;

                var data = new DockablePaneDocumentData(dockablePaneId, element, document, Get(dockablePaneId));
                documentProvider?.DockablePaneChanged(data);
            }
        }

        private void Application_Idling(object sender, Autodesk.Revit.UI.Events.IdlingEventArgs e)
        {
            var uiapp = sender as UIApplication;

            if (ActiveDocumentChanged(uiapp))
            {
                foreach (var dpid in paneIdFrameworkElements.Keys)
                {
                    ExecutePaneDocumentProviderChanged(uiapp, dpid);
                }
            }

            while (dockablePaneIds.Dequeue() is DockablePaneId dpid)
            {
                ExecutePaneDocumentProviderChanged(uiapp, dpid);
            }
        }

        #region ActiveDocument
        private bool IsEquals(Autodesk.Revit.DB.Document a, Autodesk.Revit.DB.Document b)
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;

            if (!a.IsValidObject || !b.IsValidObject)
                return false;

            return a.GetHashCode() == b.GetHashCode();
        }

        private Autodesk.Revit.DB.Document ActiveDocument;
        private bool ActiveDocumentChanged(UIApplication uiapp)
        {
            var document = uiapp.ActiveUIDocument?.Document;

            if (IsEquals(document, ActiveDocument))
                return false;

            ActiveDocument = document;

            return true;
        }
        #endregion

        #region Register / Get
        public bool Register<T>(Guid guid, T element) where T : FrameworkElement
        {
            return Register(guid, null, element, (IDockablePaneProvider)null);
        }

        //public bool Register<T>(Guid guid, T element, Action<DockablePaneProviderData> config) where T : FrameworkElement
        //{
        //    return Register(guid, null, element, new DockablePaneProvider(config));
        //}

        public bool Register<T>(Guid guid, T element, IDockablePaneProvider dockablePaneProvider) where T : FrameworkElement
        {
            return Register(guid, null, element, dockablePaneProvider);
        }

        //public bool Register<T>(Guid guid, string title, T element) where T : FrameworkElement
        //{
        //    return Register(guid, title, element, (IDockablePaneProvider)null);
        //}

        //public bool Register<T>(Guid guid, string title, T element, Action<DockablePaneProviderData> config) where T : FrameworkElement
        //{
        //    return Register(guid, title, element, new DockablePaneProvider(config));
        //}

        public bool Register<T>(Guid guid, string title, T element,
            IDockablePaneProvider dockablePaneProvider = null,
            IDockablePaneDocumentProvider dockableDocumentPaneProvider = null) where T : FrameworkElement
        {
            var dpid = new DockablePaneId(guid);
            if (DockablePane.PaneIsRegistered(dpid) == false)
            {
                if (string.IsNullOrWhiteSpace(title))
                    if (element is Page page)
                        title = page.Title;

                if (string.IsNullOrWhiteSpace(title))
                    title = element.ToString();

                if (dockablePaneProvider is null)
                    dockablePaneProvider = element as IDockablePaneProvider;

                if (dockableDocumentPaneProvider is null)
                    dockableDocumentPaneProvider = element as IDockablePaneDocumentProvider;

                if (dockableDocumentPaneProvider is null)
                    dockableDocumentPaneProvider = dockablePaneProvider as IDockablePaneDocumentProvider;

                try
                {
                    var creator = new DockablePaneProviderCreator(element, dockablePaneProvider);
                    application.RegisterDockablePane(dpid, title, creator);
                    paneIdFrameworkElements[dpid] = element;
                    dockablePaneDocumentProvider[dpid] = dockableDocumentPaneProvider;
                }
                catch { }
            }
            return DockablePane.PaneIsRegistered(dpid);
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
        #endregion
    }
}
