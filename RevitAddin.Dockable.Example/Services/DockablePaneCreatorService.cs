using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace RevitAddin.Dockable.Example.Services
{
    /// <summary>
    /// DockablePaneCreatorService
    /// </summary>
    public class DockablePaneCreatorService : IDisposable
    {
        private readonly UIControlledApplication application;
        private readonly Dictionary<DockablePaneId, FrameworkElement> paneIdFrameworkElements;
        private readonly Dictionary<DockablePaneId, IDockablePaneDocumentProvider> dockablePaneDocumentProvider;
        private bool HasInitialized;

        /// <summary>
        /// DockablePaneCreatorService
        /// </summary>
        /// <param name="application"></param>
        public DockablePaneCreatorService(UIControlledApplication application)
        {
            this.application = application;
            this.paneIdFrameworkElements = new Dictionary<DockablePaneId, FrameworkElement>();
            this.dockablePaneDocumentProvider = new Dictionary<DockablePaneId, IDockablePaneDocumentProvider>();
        }

        /// <summary>
        /// Initialize
        /// </summary>
        /// <remarks>Register the <see cref="UIControlledApplication.DockableFrameVisibilityChanged"/> and <see cref="UIControlledApplication.Idling"/>.</remarks>
        public void Initialize()
        {
            if (HasInitialized)
                return;

            HasInitialized = true;

            application.DockableFrameVisibilityChanged += Application_DockableFrameVisibilityChanged;
            application.Idling += Application_Idling;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <remarks>Unregister the <see cref="UIControlledApplication.DockableFrameVisibilityChanged"/> and <see cref="UIControlledApplication.Idling"/>.</remarks>
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

        #region Events
        Queue<DockablePaneId> dockablePaneIdsChanged = new Queue<DockablePaneId>();
        private void Application_DockableFrameVisibilityChanged(object sender, Autodesk.Revit.UI.Events.DockableFrameVisibilityChangedEventArgs e)
        {
            var paneId = e.PaneId;
            dockablePaneIdsChanged.Enqueue(paneId);
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

            while (dockablePaneIdsChanged.Dequeue() is DockablePaneId dpid)
            {
                ExecutePaneDocumentProviderChanged(uiapp, dpid);
            }
        }
        #endregion

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
        /// <summary>
        /// Adds a new dockable pane to the Revit user interface using <paramref name="element"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="guid">Unique identifier for the new pane.</param>
        /// <param name="element">The Windows Presentation Framework object containing the pane's user interface.</param>
        /// <remarks>By default, is register the <paramref name="element"/> as the intefaces <see cref="Autodesk.Revit.UI.IDockablePaneProvider"/> and/or <see cref="IDockablePaneDocumentProvider"/>.</remarks>
        public bool Register<T>(Guid guid, T element) where T : FrameworkElement
        {
            return Register(guid, null, element);
        }
        /// <summary>
        /// Adds a new dockable pane to the Revit user interface using <paramref name="element"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="guid">Unique identifier for the new pane.</param>
        /// <param name="element">The Windows Presentation Framework object containing the pane's user interface.</param>
        /// <param name="dockablePaneProvider">Interface that the Revit UI will call during initialization of the user interface to gather information about add-in dockable pane windows.</param>
        /// <remarks>By default, is register the <paramref name="element"/> as the intefaces <see cref="Autodesk.Revit.UI.IDockablePaneProvider"/> and/or <see cref="IDockablePaneDocumentProvider"/>.</remarks>
        public bool Register<T>(Guid guid, T element, IDockablePaneProvider dockablePaneProvider) where T : FrameworkElement
        {
            return Register(guid, null, element, dockablePaneProvider);
        }

        /// <summary>
        /// Adds a new dockable pane to the Revit user interface using <paramref name="element"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="guid">Unique identifier for the new pane.</param>
        /// <param name="element">The Windows Presentation Framework object containing the pane's user interface.</param>
        /// <param name="dockableDocumentPaneProvider">Interface that the Revit UI will call during the idling about add-in dockable pane windows.</param>
        /// <remarks>By default, is register the <paramref name="element"/> as the intefaces <see cref="Autodesk.Revit.UI.IDockablePaneProvider"/> and/or <see cref="IDockablePaneDocumentProvider"/>.</remarks>
        public bool Register<T>(Guid guid, T element, IDockablePaneDocumentProvider dockableDocumentPaneProvider) where T : FrameworkElement
        {
            return Register(guid, null, element, null, dockableDocumentPaneProvider);
        }

        /// <summary>
        /// Adds a new dockable pane to the Revit user interface using <paramref name="element"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="guid">Unique identifier for the new pane.</param>
        /// <param name="element">The Windows Presentation Framework object containing the pane's user interface.</param>
        /// <param name="dockablePaneProvider">Interface that the Revit UI will call during initialization of the user interface to gather information about add-in dockable pane windows.</param>
        /// <param name="dockableDocumentPaneProvider">Interface that the Revit UI will call during the idling about add-in dockable pane windows.</param>
        /// <remarks>By default, is register the <paramref name="element"/> as the intefaces <see cref="Autodesk.Revit.UI.IDockablePaneProvider"/> and/or <see cref="IDockablePaneDocumentProvider"/>.</remarks>
        public bool Register<T>(Guid guid, T element, IDockablePaneProvider dockablePaneProvider, IDockablePaneDocumentProvider dockableDocumentPaneProvider) where T : FrameworkElement
        {
            return Register(guid, null, element, dockablePaneProvider, dockableDocumentPaneProvider);
        }
        /// <summary>
        /// Adds a new dockable pane to the Revit user interface using <paramref name="element"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="guid">Unique identifier for the new pane.</param>
        /// <param name="title">String to use for the pane caption.</param>
        /// <param name="element">The Windows Presentation Framework object containing the pane's user interface.</param>
        /// <param name="dockableDocumentPaneProvider">Interface that the Revit UI will call during the idling about add-in dockable pane windows.</param>
        /// <remarks>By default, is register the <paramref name="element"/> as the intefaces <see cref="Autodesk.Revit.UI.IDockablePaneProvider"/> and/or <see cref="IDockablePaneDocumentProvider"/>.</remarks>
        public bool Register<T>(Guid guid, string title, T element, IDockablePaneDocumentProvider dockableDocumentPaneProvider) where T : FrameworkElement
        {
            return Register(guid, title, element, null, dockableDocumentPaneProvider);
        }
        /// <summary>
        /// Adds a new dockable pane to the Revit user interface using <paramref name="element"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="guid">Unique identifier for the new pane.</param>
        /// <param name="title">String to use for the pane caption.</param>
        /// <param name="element">The Windows Presentation Framework object containing the pane's user interface.</param>
        /// <param name="dockablePaneProvider">Interface that the Revit UI will call during initialization of the user interface to gather information about add-in dockable pane windows.</param>
        /// <param name="dockableDocumentPaneProvider">Interface that the Revit UI will call during the idling about add-in dockable pane windows.</param>
        /// <remarks>By default, is register the <paramref name="element"/> as the intefaces <see cref="Autodesk.Revit.UI.IDockablePaneProvider"/> and/or <see cref="IDockablePaneDocumentProvider"/>.</remarks>
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
        /// <summary>
        /// Gets a DockablePane object by its ID.
        /// </summary>
        /// <param name="guid">Unique identifier for the new pane.</param>
        /// <remarks>Return null if not register or not available.</remarks>
        public DockablePane Get(Guid guid)
        {
            var dpid = new DockablePaneId(guid);
            return Get(dpid);
        }
        /// <summary>
        /// Gets a DockablePane object by its ID.
        /// </summary>
        /// <param name="dpid">Identifier for a pane that participates in the Revit docking window system.</param>
        /// <remarks>Return null if not register or not available.</remarks>
        public DockablePane Get(DockablePaneId dpid)
        {
            try
            {
                return application.GetDockablePane(dpid);
            }
            catch { }
            return null;
        }
        /// <summary>
        /// Get a FrameworkElement registered by its ID.
        /// </summary>
        /// <param name="guid">Unique identifier for the new pane.</param>
        /// <remarks>Return null if not register or not available.</remarks>
        public FrameworkElement GetFrameworkElement(Guid guid)
        {
            var dpid = new DockablePaneId(guid);
            return GetFrameworkElement(dpid);
        }
        /// <summary>
        /// Get a FrameworkElement registered by its ID.
        /// </summary>
        /// <param name="dpid">Identifier for a pane that participates in the Revit docking window system.</param>
        /// <remarks>Return null if not register or not available.</remarks>
        public FrameworkElement GetFrameworkElement(DockablePaneId dpid)
        {
            if (this.paneIdFrameworkElements.TryGetValue(dpid, out FrameworkElement element))
                return element;

            return null;
        }
        #endregion
    }
}
