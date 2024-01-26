using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows;

namespace RevitAddin.Dockable.Example.Services
{
    /// <summary>
    /// Interface that the Revit UI will call during the idling about add-in dockable pane windows.
    /// </summary>
    public interface IDockablePaneDocumentProvider
    {
        /// <summary>
        /// This method is called when the DockablePane change state. When the visibility of the DockablePane changed, or a document changed inside Revit UI.
        /// </summary>
        /// <param name="data"></param>
        /// <remarks>This method is executed inside the <see cref="Autodesk.Revit.UI.UIApplication.Idling"/> event.</remarks>
        void DockablePaneChanged(DockablePaneDocumentData data);
    }

    /// <summary>
    /// DockablePaneDocumentData
    /// </summary>
    public sealed class DockablePaneDocumentData
    {
        /// <summary>
        /// DockablePaneDocumentData
        /// </summary>
        /// <param name="dockablePaneId"></param>
        /// <param name="frameworkElement"></param>
        /// <param name="document"></param>
        /// <param name="dockablePane"></param>
        internal DockablePaneDocumentData(DockablePaneId dockablePaneId, FrameworkElement frameworkElement, Document document, DockablePane dockablePane)
        {
            DockablePaneId = dockablePaneId;
            FrameworkElement = frameworkElement;
            Document = document;
            DockablePane = dockablePane;
        }
        /// <summary>
        /// DockablePaneId related to the DockablePane.
        /// </summary>
        public DockablePaneId DockablePaneId { get; }
        /// <summary>
        /// The FrameworkElement that is used to display the DockablePane.
        /// </summary>
        public FrameworkElement FrameworkElement { get; }
        /// <summary>
        /// Document, this could be null if no document is opened.
        /// </summary>
        public Document Document { get; }
        /// <summary>
        /// DockablePane related to the DockablePaneId, this could be null if no document is opened.
        /// </summary>
        public DockablePane DockablePane { get; }
    }
}
