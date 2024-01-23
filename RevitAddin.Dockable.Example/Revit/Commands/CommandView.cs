using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitAddin.Dockable.Example.Views;
using System.Windows.Controls;

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

    [Transaction(TransactionMode.Manual)]
    public class CommandViewStatic : IExternalCommand
    {
        private static PageView _pageView;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIApplication uiapp = commandData.Application;

            if (_pageView is null)
            {
                var page = new DockablePage2();
                _pageView = new PageView(page);
                _pageView.Closed += (s, e) => _pageView = null;
            }

            if (_pageView.IsVisible)
                _pageView.Hide();
            else
                _pageView.Show();

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class CommandBackground : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIApplication uiapp = commandData.Application;

            if (App.DockablePaneCreatorService.GetFrameworkElement(DockablePage2.Guid) is Page page)
            {
                page.Background = System.Windows.Media.Brushes.WhiteSmoke;
            }

            if (App.DockablePaneCreatorService.GetFrameworkElement(DockablePage2.Guid3) is Page page3)
            {
                page3.Background = System.Windows.Media.Brushes.WhiteSmoke;
            }

            return Result.Succeeded;
        }
    }

}
