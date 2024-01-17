using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitAddin.Dockable.Example.Services;
using RevitAddin.Dockable.Example.Views;

namespace RevitAddin.Dockable.Example.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class CommandShow : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIApplication uiapp = commandData.Application;

            App.DockablePaneService.Get(DockablePage.Guid)?.Show();

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class CommandShow2 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIApplication uiapp = commandData.Application;

            App.DockablePaneService.Get(DockablePage.Guid2)?.Show();

            return Result.Succeeded;
        }
    }

}
