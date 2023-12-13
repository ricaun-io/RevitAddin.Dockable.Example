using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitAddin.Dockable.Example.Views;

namespace RevitAddin.Dockable.Example.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class CommandView : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIApplication uiapp = commandData.Application;

            new PageView(new DockablePage()).Show();

            return Result.Succeeded;
        }
    }

}
