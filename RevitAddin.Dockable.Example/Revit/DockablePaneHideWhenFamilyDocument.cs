using RevitAddin.Dockable.Example.Services;
using System;

namespace RevitAddin.Dockable.Example.Revit
{
    public class DockablePaneHideWhenFamilyDocument : IDockablePaneDocumentProvider
    {
        public void DockablePaneChanged(DockablePaneDocumentData data)
        {
            //Console.WriteLine($"{data.DockablePaneId.Guid} \t {data.DockablePane.TryGetTitle()} - {data.DockablePane.TryIsShown()} \t {data.Document?.Title} \t {data.FrameworkElement}");

            var isFamilyDocument = data.Document?.IsFamilyDocument == true;

            if (isFamilyDocument)
            {
                data.DockablePane.TryHide();
            }

        }
    }

}