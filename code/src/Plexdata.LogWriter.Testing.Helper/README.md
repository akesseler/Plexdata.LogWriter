
## Plexdata.LogWriter.Testing.Helper

This project represents an executable to be able to perform 
live testings of all logging interfaces. For the moment, there 
are a few examples that show how to use the loggers. See below 
for these examples.

### Standard Console Logger Example 

An example of how to use standard console logger.

```
using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Extensions;
using Plexdata.LogWriter.Facades.Standard;
using Plexdata.LogWriter.Logging.Standard;
using Plexdata.LogWriter.Settings;
using System;

namespace Plexdata.LogWriter.Examples
{
    class Program
    {
        static void Main(String[] args)
        {
            IConsoleLoggerFacade facade = new ConsoleLoggerFacade();
            IConsoleLoggerSettings settings = new ConsoleLoggerSettings
            {
                WindowTitle = "Console Logger Test Application",
                UseColors = true,
                PartSplit = '#',
                LogLevel = LogLevel.Trace,
                QuickEdit = true, // Does not have any effect in this context...
                BufferSize = new Dimension(150, 1000),
            };

            settings.Coloring[LogLevel.Critical] = new Coloring(ConsoleColor.Yellow, ConsoleColor.DarkCyan);

            (String, Object)[] details = new (String, Object)[]
            {
                ("Active", true), ("Average", 12345.67M), ("Name", "Details")
            };

            IConsoleLogger logger = new ConsoleLogger(settings, facade);

            logger.Debug("This is a Debug logging entry.", details);
            logger.Trace("This is a Trace logging entry.", details);
            logger.Verbose("This is a Verbose logging entry.", details);
            logger.Message("This is a Message logging entry.", details);
            logger.Warning("This is a Warning logging entry.", details);
            logger.Error("This is a Error logging entry.", details);
            logger.Fatal("This is a Fatal logging entry.", details);
            logger.Critical("This is a Critical logging entry.", details);

            Console.Write("Hit any key to finish... ");
            Console.ReadKey();
        }
    }
}
```

### Windows Console Logger Example 

An example of how to use console logger for Windows.

```
using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Extensions;
using Plexdata.LogWriter.Facades.Windows;
using Plexdata.LogWriter.Logging.Windows;
using Plexdata.LogWriter.Settings;
using System;

namespace Plexdata.LogWriter.Examples
{
    class Program
    {
        static void Main(String[] args)
        {
            IConsoleLoggerFacade facade = new ConsoleLoggerFacade();
            IConsoleLoggerSettings settings = new ConsoleLoggerSettings
            {
                WindowTitle = "Console Logger Test Application",
                UseColors = true,
                PartSplit = '#',
                LogLevel = LogLevel.Trace,
                QuickEdit = true,
                BufferSize = new Dimension(150, 1000),
            };

            settings.Coloring[LogLevel.Critical] = new Coloring(ConsoleColor.Yellow, ConsoleColor.DarkCyan);

            (String, Object)[] details = new (String, Object)[]
            {
                ("Active", true), ("Average", 12345.67M), ("Name", "Details")
            };

            IConsoleLogger logger = new ConsoleLogger(settings, facade);

            logger.Debug("This is a Debug logging entry.", details);
            logger.Trace("This is a Trace logging entry.", details);
            logger.Verbose("This is a Verbose logging entry.", details);
            logger.Message("This is a Message logging entry.", details);
            logger.Warning("This is a Warning logging entry.", details);
            logger.Error("This is a Error logging entry.", details);
            logger.Fatal("This is a Fatal logging entry.", details);
            logger.Critical("This is a Critical logging entry.", details);

            Console.Write("Hit any key to finish... ");
            Console.ReadKey();
        }
    }
}
```
