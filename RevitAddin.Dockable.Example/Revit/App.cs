using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitAddin.Dockable.Example.Revit.Commands;
using RevitAddin.Dockable.Example.Services;
using RevitAddin.Dockable.Example.Views;
using ricaun.Revit.UI;
using System.Threading.Tasks;

namespace RevitAddin.Dockable.Example.Revit
{
    [AppLoader]
    public class App : IExternalApplication
    {
        public static DockablePaneCreatorService DockablePaneCreatorService;
        private static RibbonPanel ribbonPanel;
        public Result OnStartup(UIControlledApplication application)
        {
            DockablePaneCreatorService = new DockablePaneCreatorService(application);
            DockablePaneCreatorService.Initialize();


            application.ControlledApplication.ApplicationInitialized += (sender, args) =>
            {
                //DockablePaneService.Register<DockablePage>(DockablePage.Guid);

                DockablePaneCreatorService.Register(DockablePage.Guid, "DockablePage", new DockablePage());

                // DockablePage2
                {
                    var page = new DockablePage2();
                    DockablePaneCreatorService.Register(DockablePage2.Guid, "DockablePage2", page, new DockablePaneHideWhenFamilyDocument());
                }

                {
                    var page = new DockablePage2();
                    page.Title = "DockablePage3";
                    DockablePaneCreatorService.Register(DockablePage2.Guid3, page);
                }
            };

            ribbonPanel = application.CreatePanel("Dockable");

            var commandShow = ribbonPanel.CreatePushButton<CommandShow>("Show")
                .SetLargeImage("Resources/revit.ico");
            var commandHide = ribbonPanel.CreatePushButton<CommandHide>("Hide")
                .SetLargeImage("Resources/revit.ico");
            var commandView = ribbonPanel.CreatePushButton<CommandView>("View")
                .SetLargeImage("Resources/revit.ico");

            ribbonPanel.RowStackedItems(commandShow, commandHide, commandView);

            ribbonPanel.RowStackedItems(
                    ribbonPanel.CreatePushButton<CommandShow2>("Show2").SetLargeImage("Resources/revit.ico"),
                    ribbonPanel.CreatePushButton<CommandHide2>("Hide2").SetLargeImage("Resources/revit.ico"),
                    ribbonPanel.CreatePushButton<CommandBackground>("Background").SetLargeImage("Resources/revit.ico"),
                    ribbonPanel.CreatePushButton<CommandViewStatic>("ViewStatic").SetLargeImage("Resources/revit.ico")
                );

            application.DockableFrameVisibilityChanged += Application_DockableFrameVisibilityChanged;

            return Result.Succeeded;
        }

        private void Application_DockableFrameVisibilityChanged(object sender, Autodesk.Revit.UI.Events.DockableFrameVisibilityChangedEventArgs e)
        {
            //Console.WriteLine($"{e.PaneId.Guid} {e.DockableFrameShown}");
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            ribbonPanel?.Remove();

            DockablePaneCreatorService.Dispose();

            application.DockableFrameVisibilityChanged -= Application_DockableFrameVisibilityChanged;

            return Result.Succeeded;
        }
    }

}