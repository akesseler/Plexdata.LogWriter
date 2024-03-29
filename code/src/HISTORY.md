
**1.0.7.1**
- Usage of package `Plexdata.Utilities.Testing`.
- Package updates (NUnit and other).
- Introduction of new logging level `Disaster`.
- Introduction of logging type `GELF` including a proper formatter.
- Adding of project `Plexdata.LogWriter.Network`.
- File `.editorconfig` updated.
- Copyright year changed in all files.
- Version number increased.

**1.0.6.1**
- File `.editorconfig` added.
- Framework updates (.NET Framework 4.8, .NET Core 3.1).
- Help projects and readmes updated.
- NuGet icon reference changed from _URL_ into _Pack_.
- Minor fixings in tests and some files renamed.
- Copyright year changed in all files.
- Logger extension methods split into different files.
- Support of type `Guid` as `TScope`. 
- Introduction of method `BeginScope()`.
- Review of source code documentation.
- Version number increased.
- Wiki and release update on _GitHub_.
- All affected packages released on `nuget.org`.

**1.0.5.2**
- Support of a simple XML message format.
- Correction of some typos.
- Tests adjusted as well as new tests added.
- Testing application enriched by new feature and others.
- Source code and online documentation updated.
- Version number increased.
- Wiki and release update on _GitHub_.
- All affected packages released on `nuget.org`.

**1.0.5.1**
- Introduction of e-mail logger.
  - Mail logger project added.
  - Mail logger testing project added.
- All package release notes replaced by project history hints.
- Source code and online documentation updated.
- Version number increased.
- Wiki and release update on _GitHub_.
- All affected packages released on `nuget.org`.

**1.0.4.3**
- Interface `ILoggerSettingsSection` extended by method `GetValues()`.
- Tests for `ILoggerSettingsSection` derived classes have been adjusted.
- Class `LoggerSettings` extended by method `GetSectionValues()`.
- Missing tests added to `LoggerSettingsTests`.
- Version number increased.

**1.0.4.2**
- Minor changes.
  - In-text code block quotation in various readme files adjusted.
  - Readme file for project `WindowsFormsStreamLoggerTestApplication` added.
  - Dependencies for project `Plexdata.LogWriter.Help.Producer` adjusted.
- Bugfix in composite logger.
  - Using settings to allow logger basic setup.
- Source code and online documentation updated.
- Version number increased.
- Wiki and release update on _GitHub_.
- All affected packages released on `nuget.org`.

**1.0.4.1**
- Introduction of composite logger.
  - Composite logger project added.
  - Composite logger testing project added.
- Source code and online documentation updated.
- Version number increased.
- Wiki and release update on _GitHub_.
- All affected packages released on `nuget.org`.

**1.0.3.2**
- Constructors of class `StreamLoggerSettings` extended by a `Stream` parameter.
- Tests for class `StreamLoggerSettings` have been adjusted.
- Support of additional features, such as `LoggerStream` (and its event related implementations).
- Tests for class `LoggerStream` and related implementations created.
- Source code documentation updated.
- Upgrade of _NUnit_ packages.
- Version number increased.
- Wiki and release update on _GitHub_.
- All affected packages released on `nuget.org`.

**1.0.3.1**
- Introduction of stream logger.
  - Stream logger project added.
  - Stream logger testing project added.
  - Stream logger example project added.
- Source code and online documentation updated.
- Version number increased.
- Wiki and release update on _GitHub_.
- All affected packages released on `nuget.org`.

**1.0.2.7**
- Bugfix in `JsonFormatter`, re-escaping backslashes in any character escaping within double-quoted string value results.
- Minor changes in exception handling in class `PersistentLoggerFacade`.
- Correction of typos and documentation text adaptations.
- Missing tests added and existing tests adjusted.
- Version number increased.

**1.0.2.6**
- Persistent logger changes:
  - Support of auto-creation for non-existing folders.
  - Support of fallback folder if no directory is part of the filename.
  - Adjustment of related tests.
- Update of general and source code documentation.
- Update of content of major `readme.md` file:
  - Adding of section _Downloads_.
  - Adding of `shields.io` tags.
- Package-license-URL replaced by package-license-file `LICENSE.md` in each of the _Publish Packages_.
- Package release notes updated.
- Version number increased.
- Wiki, release and `docs` update on _GitHub_.
- All affected packages released on `nuget.org`.

**1.0.2.5**
- Filename of persistent logger settings supports environment variables.
- Tests for expanding environment variables in filenames added.
- Tests for invalid conversion formats added.
- Update of general and source code documentation.
- Package release notes updated.
- Version number increased.
- Wiki and `docs` update on _GitHub_.

**1.0.2.4**
- Setting up a new release for `nuget.org` because of being unable to update the previous verion.
- Version number increased.
- Wiki and release update on _GitHub_.
- All affected packages released on `nuget.org` (release notes update).

**1.0.2.3**
- Support of settings construction by using interface `ILoggerSettingsSection`.
- Extending the tests for `ILoggerSettingsSection` usage and related implementations.
- Bugfix in class `ConsoleLoggerFacade` catching any exception in property `BufferSize`.
- Adding of project `Plexdata.LogWriter.Testing.Helper.Net.Core`.
- Adding of project `Plexdata.LogWriter.Testing.Helper.Asp.Core`.
- Update of source code documentation.
- Version number increased.
- Wiki and release update on _GitHub_.
- All affected packages released on `nuget.org` (release notes update).

**1.0.2.2**
- Bugfix in class `SettingsPoliciesExtension` for toggling file names when rolling-mode is on.
- Version number increased.

**1.0.2.1 (BREAKING CHANGES)**
- The constructor with filename of class `PersistentLoggerSettings` has been removed at all.
- The property `Filename` of interface `IPersistentLoggerSettings` gots a public setter.
- Tests for class `PersistentLoggerSettings` have been adjusted.
- Private constant fields with appropriated default values have been added to each of the settings classes.
- Version number increased.

**1.0.1.2**
- Some minor fixes and changes.
- Introduction of persistent logger.
- Update of source code documentation.
- Version number increased.
- Wiki and release update on _GitHub_.
- All affected packages released on `nuget.org`.

**1.0.1.1**
- Project icon changed.
- Version number increased.

**1.0.1.0**

- Restructuring of all namespaces (breaking changes).
- Some renamings and minor changes.
- Removing of unused code.
- Update of source code documentation.
- Wiki and release update on _GitHub_.
- Version number increased.

**1.0.0.3**

- Adding of queuing support and related tests.
- Update of source code documentation.
- Wiki and release update on _GitHub_.
- Version number increased.

**1.0.0.2**

- Adding of interface `INotifyPropertyChanged` to logger settings.
- General code refactoring (moving redundant code into base classes).
- Bug fixing as well as adding and updating of tests.
- Update of source code documentation.
- Version number increased.

**1.0.0.1**

- Configuration of NuGet information in all relevant projects.
- Replacing the .NET Framework project for the Windows logger by a .NET Standard project.
- Reviewing and rebuilding Help and Wiki documentation.
- Version number increased.
- All affected packages released on `nuget.org`.

**1.0.0.0**

- Initial draft.
- Published on [https://github.com/akesseler/Plexdata.LogWriter](https://github.com/akesseler/Plexdata.LogWriter).
