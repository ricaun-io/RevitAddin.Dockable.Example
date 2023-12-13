using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitAddin.Dockable.Example.Revit.Commands;
using RevitAddin.Dockable.Example.Services;
using RevitAddin.Dockable.Example.Views;
using ricaun.Revit.UI;
using System;

namespace RevitAddin.Dockable.Example.Revit
{
    [AppLoader]
    public class App : IExternalApplication
    {
        public static DockablePaneService DockablePaneService;
        private static RibbonPanel ribbonPanel;
        public Result OnStartup(UIControlledApplication application)
        {
            DockablePaneService = new DockablePaneService(application);
            application.ControlledApplication.ApplicationInitialized += (sender, args) =>
            {
                DockablePaneService.Register<DockablePage>(DockablePage.Guid);
            };

            ribbonPanel = application.CreatePanel("Dockable");

            var commandShow = ribbonPanel.CreatePushButton<CommandShow>("Show")
                .SetLargeImage("Resources/revit.ico");
            var commandHide = ribbonPanel.CreatePushButton<CommandHide>("Hide")
                .SetLargeImage("Resources/revit.ico");
            var commandView = ribbonPanel.CreatePushButton<CommandView>("View")
                .SetLargeImage("Resources/revit.ico");

            ribbonPanel.RowStackedItems(commandShow, commandHide, commandView);

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            ribbonPanel?.Remove();
            return Result.Succeeded;
        }
    }

}