# RevitAddin.Dockable.Example

[![Revit 2019](https://img.shields.io/badge/Revit-2019+-blue.svg)](../..)
[![Visual Studio 2022](https://img.shields.io/badge/Visual%20Studio-2022-blue)](../..)
[![Nuke](https://img.shields.io/badge/Nuke-Build-blue)](https://nuke.build/)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Build](../../actions/workflows/Build.yml/badge.svg)](../../actions)

This project shows how to register and show/hide a `DockablePane` using Revit API.

## DockablePaneService

The `DockablePaneService` class implements some methods to `Register` and `Get` the `DockablePane` registered in Revit.

### Register

To `Register` a `DockablePane` in Revit, you need to provide the `Guid` of the `DockablePane` and the `Page` with the interface `IDockablePaneProvider`.

**The `Register` of a `DockablePane` only works before Revit finish initialize, or in the `ApplicationInitialized` event.**

```C#
public class App : IExternalApplication
{
    public static DockablePaneService DockablePaneService;
    public Result OnStartup(UIControlledApplication application)
    {
        DockablePaneService = new DockablePaneService(application);
        application.ControlledApplication.ApplicationInitialized += (sender, args) =>
        {
            DockablePaneService.Register<DockablePage>(DockablePage.Guid);
        };
        return Result.Succeeded;
    }

    public Result OnShutdown(UIControlledApplication application)
    {
        return Result.Succeeded;
    }
}
```

### Get

To `Get` a `DockablePane` in Revit, you need to provide the `Guid` of the `DockablePane` or the `Page` registerd.
```C#
DockablePane dockablePane = App.DockablePaneService.Get(DockablePage.Guid);
```
or
```C#
DockablePane dockablePane = App.DockablePaneService.Get<DockablePage>();
```
*This method gonna return `null` if the `DockablePane` is not registered.*

## Installation

* Download and install [RevitAddin.Dockable.Example.exe](../../releases/latest/download/RevitAddin.Dockable.Example.zip)

## License

This project is [licensed](LICENSE) under the [MIT Licence](https://en.wikipedia.org/wiki/MIT_License).

---

Do you like this project? Please [star this project on GitHub](../../stargazers)!