using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace RevitAddin.Dockable.Example.Services
{
    public class DockablePaneService
    {
        private readonly UIControlledApplication application;
        private readonly Dictionary<Type, DockablePaneId> typeDockablePaneId;
        public DockablePaneService(UIControlledApplication application)
        {
            this.application = application;
            typeDockablePaneId = new Dictionary<Type, DockablePaneId>();
        }

        public bool Register<T>(Guid guid, T dockablePane) where T : IDockablePaneProvider
        {
            return Register(guid, null, dockablePane);
        }

        public bool Register<T>(Guid guid, string title, T dockablePane) where T : IDockablePaneProvider
        {
            var dpid = new DockablePaneId(guid);
            if (DockablePane.PaneIsRegistered(dpid) == false)
            {
                if (string.IsNullOrWhiteSpace(title))
                    if (dockablePane is Page page)
                        title = page.Title;

                if (string.IsNullOrWhiteSpace(title))
                    title = dockablePane.ToString();

                try
                {
                    application.RegisterDockablePane(dpid, title, dockablePane);
                    typeDockablePaneId[dockablePane.GetType()] = dpid;
                }
                catch { }
            }
            return DockablePane.PaneIsRegistered(dpid);
        }

        public bool Register<T>(Guid guid, string title) where T : IDockablePaneProvider, new()
        {
            var dockablePane = new T();
            return Register(guid, title, dockablePane);
        }

        public bool Register<T>(Guid guid) where T : IDockablePaneProvider, new()
        {
            return Register<T>(guid, null);
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

        public DockablePane Get<T>() where T : IDockablePaneProvider
        {
            var type = typeof(T);
            if (typeDockablePaneId.TryGetValue(type, out DockablePaneId dpid))
                return Get(dpid);
            return null;
        }
    }
}
