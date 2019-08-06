
## Plexdata.LogWriter.Testing.Helper.Asp.Core

This special project represents a testing environment to be able 
to test the behaviour of dependency injection especially for the 
usage of interface `ILoggerSettingsSection`.

Furthermore, this project includes the settings file `appsettings.json` 
file that show how to configure the logging writer with an external 
configuration. 

Please note, it is necessary to publish the project to get it running 
as executable outside of Visual Studio. This means in turn, debugging 
inside of Visual Studio is NOT possible without publishing.

### Executing Example

- Build the whole project.
- Run publishing this project.
- Open file explorer and go to publish folder.
- Start `Plexdata.LogWriter.Testing.Helper.Asp.Core.exe`.
- Open an internet browser and type URL `https://localhost:5001/api/values`
- Thereafter, the result should be
  - In the internet browser you should see the result `["value1","value2"]`.
  - In the assigned console window you should see a Trace message.

### Dependency Injection Example

Below an example of how to inject an external configuration into the 
console logger in a ASP.NET Core environment.

In class `Startup` in method `ConfigureServices` add lines shown below.

```
ILoggerSettingsBuilder builder = new LoggerSettingsBuilder();
builder.SetFilename("appsettings.json");

services.AddSingleton<ILoggerSettingsSection>(builder.Build());
services.AddSingleton<IConsoleLogger, ConsoleLogger>();
services.AddSingleton<IConsoleLoggerSettings, ConsoleLoggerSettings>();
```

In class `ValuesController` add a constructor like shown here.

```
private readonly IConsoleLogger logger = null;

public ValuesController(IConsoleLogger logger) : base()
{
    this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
}
```

Finally, use the logger like this.

```
public ActionResult<IEnumerable<String>> Get()
{
    ...
    this.logger.Trace("GET api/values called");
    ...
}
```
