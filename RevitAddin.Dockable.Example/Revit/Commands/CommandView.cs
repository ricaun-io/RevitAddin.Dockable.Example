using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitAddin.Dockable.Example.Views;

namespace RevitAddin.Dockable.Example.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class CommandView : IExternalCommand
    {
        private static PageView _pageView;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIApplication uiapp = commandData.Application;

            if (_pageView is null)
            {
                _pageView = new PageView(new DockablePage());
                _pageView.Closed += (s, e) => _pageView = null;
            }

            if (_pageView.IsVisible)
                _pageView.Hide();
            else
                _pageView.Show();

            return Result.Succeeded;
        }
    }
}
