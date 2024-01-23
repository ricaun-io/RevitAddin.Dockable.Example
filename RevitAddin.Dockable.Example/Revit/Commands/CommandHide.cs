using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitAddin.Dockable.Example.Services;
using RevitAddin.Dockable.Example.Views;
using System;

namespace RevitAddin.Dockable.Example.Revit.Commands
{

    [Transaction(TransactionMode.Manual)]
    public class CommandHide : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIApplication uiapp = commandData.Application;

            App.DockablePaneService.Get<DockablePage>()?.Hide();

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class CommandHide2 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIApplication uiapp = commandData.Application;

            App.DockablePaneCreatorService.Get(DockablePage2.Guid)?.Hide();
            App.DockablePaneCreatorService.Get(DockablePage2.Guid3)?.Hide();

            return Result.Succeeded;
        }
    }

}
