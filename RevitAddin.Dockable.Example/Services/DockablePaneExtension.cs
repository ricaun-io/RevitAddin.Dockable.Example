using Autodesk.Revit.UI;

namespace RevitAddin.Dockable.Example.Services
{
    /// <summary>
    /// DockablePaneExtension
    /// </summary>
    public static class DockablePaneExtension
    {
        /// <summary>
        ///  Try to show the pane that is not currently visible or in a tab, display the pane in the Revit
        ///  user interface at its last docked location.
        /// </summary>
        /// <param name="dockablePane"></param>
        /// <remarks>If <paramref name="dockablePane"/> is null or <see cref="Autodesk.Revit.UI.DockablePane.IsShown"/>, nothing happen.</remarks>
        public static void TryShow(this DockablePane dockablePane)
        {
            if (dockablePane is null)
                return;
            if (dockablePane.IsShown() == false)
                dockablePane.Show();
        }

        /// <summary>
        /// Try to hide the pane is on screen, hide it. Has no effect on built-in Revit dockable panes.
        /// </summary>
        /// <param name="dockablePane"></param>
        /// <remarks>If <paramref name="dockablePane"/> is null or not <see cref="Autodesk.Revit.UI.DockablePane.IsShown"/>, nothing happen.</remarks>
        public static void TryHide(this DockablePane dockablePane)
        {
            if (dockablePane is null)
                return;
            if (dockablePane.IsShown())
                dockablePane.Hide();
        }

        /// <summary>
        /// Returns the current title (a.k.a. window caption) of the dockable pane.
        /// </summary>
        /// <param name="dockablePane"></param>
        /// <remarks>If <paramref name="dockablePane"/> is null, <see cref="string.Empty"/> is return.</remarks>
        public static string TryGetTitle(this DockablePane dockablePane)
        {
            if (dockablePane is null)
                return string.Empty;
            return dockablePane.GetTitle();
        }

        /// <summary>
        /// Identify the pane is currently visible or in a tab.
        /// </summary>
        /// <param name="dockablePane"></param>
        /// <returns></returns>
        /// <remarks>If <paramref name="dockablePane"/> is null, false is return.</remarks>
        public static bool TryIsShown(this DockablePane dockablePane)
        {
            if (dockablePane is null)
                return false;
            return dockablePane.IsShown();
        }
    }
}
