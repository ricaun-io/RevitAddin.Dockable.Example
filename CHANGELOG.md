# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [1.0.2] / 2024-01-26
- Remove `IFrameworkElementCreator` that is not necessary.
- The Revit version could be 2017 without `IFrameworkElementCreator` reference.

## [1.0.1] / 2024-01-17 - 2024-01-26
- Add `DockablePaneProviderCreator`
- Add `DockablePaneCreatorService` with auto `Visibility` event in the `FrameworkElement`.
- Add `DockablePaneExtension`
- Add `IDockablePaneDocumentProvider` and `DockablePaneDocumentData`

## [1.0.0] / 2023-12-13
- Add `DockablePaneService`
- Add `Commands`
- Add `Views`

[vNext]: ../../compare/1.0.0...HEAD
[1.0.2]: ../../compare/1.0.1...1.0.2
[1.0.1]: ../../compare/1.0.0...1.0.1
[1.0.0]: ../../compare/1.0.0