
## Plexdata.LogWriter.Testing.Helper.Net.Core

This special project represents a testing environment to be able 
to test the behaviour of dependency injection especially for the 
usage of interface `ILoggerSettingsSection`.

Furthermore, this project includes a bunch of application settings 
file that show how to configure the logging writer with an external 
configuration. All these settings files exists as XML and as JSON 
version.

Please note, it is necessary to publish the project to get it running 
as executable outside of Visual Studio. In contrast to that, debugging 
inside of Visual Studio is possible without publishing.

### Dependency Injection Example

Below an example of how to inject an external configuration into the 
console logger in a .NET Core environment.

```
using Microsoft.Extensions.DependencyInjection;
using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Extensions;
using Plexdata.LogWriter.Logging.Standard;
using Plexdata.LogWriter.Settings;
using System;
using System.IO;

namespace Plexdata.LogWriter.Examples
{
    class Program
    {
        static void Main(String[] args)
        {
            ILoggerSettingsBuilder builder = new LoggerSettingsBuilder();
            builderbuilder.SetFilename("console-logger-settings.json");

            ILoggerSettingsSection config = builder.Build();
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<ILoggerSettingsSection>(config);
            services.AddSingleton<IConsoleLogger, ConsoleLogger>();
            services.AddSingleton<IConsoleLoggerSettings, ConsoleLoggerSettings>();

            IServiceProvider provider = services.BuildServiceProvider();
            IConsoleLogger logger = provider.GetService<IConsoleLogger>();

            logger.Trace("This is a Trace logging entry.");
            logger.Debug("This is a Debug logging entry.");
            logger.Verbose("This is a Verbose logging entry.");
            logger.Message("This is a Message logging entry.");
            logger.Warning("This is a Warning logging entry.");
            logger.Error("This is a Error logging entry.");
            logger.Fatal("This is a Fatal logging entry.");
            logger.Critical("This is a Critical logging entry.");

            Console.Write("Hit any key to finish... ");
            Console.ReadKey();
        }
    }
}
```
