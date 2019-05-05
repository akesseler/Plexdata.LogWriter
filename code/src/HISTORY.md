
**1.0.2.3**
- Adding of project ``Plexdata.LogWriter.Testing.Helper.Net.Core``.
- Adding of NuGet reference ``Microsoft.Extensions.Configuration.Abstractions`` to
  - ``Plexdata.LogWriter.Abstraction``,
  - ``Plexdata.LogWriter.Console`` and
  - ``Plexdata.LogWriter.Persistent``.
- Support of settings construction by interface ``IConfiguration``.
- Extending the tests for ``IConfiguration`` usage.
- Update of source code documentation.
- Version number increased.
- Wiki and release update on GitHub.
- All affected packages released on nuget.org (release notes update).

**1.0.2.2**
- Bugfix in class ``SettingsPoliciesExtension`` for toggling file names when rolling-mode is on.
- Version number increased.

**1.0.2.1 (BREAKING CHANGES)**
- The constructor with filename of class ``PersistentLoggerSettings`` has been removed at all.
- The property ``Filename`` of interface ``IPersistentLoggerSettings`` gots a public setter.
- Tests for class ``PersistentLoggerSettings`` have been adjusted.
- Private constant fields with appropriated default values have been added to each of the settings classes.
- Version number increased.

**1.0.1.2**
- Some minor fixes and changes.
- Introduction of persistent logger.
- Update of source code documentation.
- Version number increased.
- Wiki and release update on GitHub.
- All affected packages released on nuget.org.

**1.0.1.1**
- Project icon changed.
- Version number increased.

**1.0.1.0**

- Restructuring of all namespaces (breaking changes).
- Some renamings and minor changes.
- Removing of unused code.
- Update of source code documentation.
- Wiki and release update on GitHub.
- Version number increased.

**1.0.0.3**

- Adding of queuing support and related tests.
- Update of source code documentation.
- Wiki and release update on GitHub.
- Version number increased.

**1.0.0.2**

- Adding of interface INotifyPropertyChanged to logger settings.
- General code refactoring (moving redundant code into base classes).
- Bug fixing as well as adding and updating of tests.
- Update of source code documentation.
- Version number increased.

**1.0.0.1**

- Configuration of NuGet information in all relevant projects.
- Replacing the .NET Framework project for the Windows logger by a .NET Standard project.
- Reviewing and rebuilding Help and Wiki documentation.
- Version number increased.
- All affected packages released on nuget.org.

**1.0.0.0**

- Initial draft.
- Published on [https://github.com/akesseler/Plexdata.LogWriter](https://github.com/akesseler/Plexdata.LogWriter).
