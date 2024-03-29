﻿/*
 * MIT License
 * 
 * Copyright (c) 2023 plexdata.de
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;

namespace Plexdata.LogWriter.Testing.Helper
{
    class Program
    {
        static void Main(String[] args)
        {
            // See examples below...

            Console.Write("Hit any key to finish... ");
            Console.ReadKey();
            Console.Write(Environment.NewLine);
        }
    }
}

/* An example of how to use console logger for Windows.
 * 
 * using Plexdata.LogWriter.Abstraction;
 * using Plexdata.LogWriter.Definitions;
 * using Plexdata.LogWriter.Extensions;
 * using Plexdata.LogWriter.Facades.Windows;
 * using Plexdata.LogWriter.Logging.Windows;
 * using Plexdata.LogWriter.Settings;
 * using System;
 * 
 * namespace Plexdata.LogWriter.Examples
 * {
 *     class Program
 *     {
 *         static void Main(String[] args)
 *         {
 *             IConsoleLoggerFacade facade = new ConsoleLoggerFacade();
 *             IConsoleLoggerSettings settings = new ConsoleLoggerSettings
 *             {
 *                 WindowTitle = "Console Logger Test Application",
 *                 UseColors = true,
 *                 PartSplit = '#',
 *                 LogLevel = LogLevel.Trace,
 *                 QuickEdit = true,
 *                 BufferSize = new Dimension(150, 1000),
 *             };
 * 
 *             settings.Coloring[LogLevel.Critical] = new Coloring(ConsoleColor.Yellow, ConsoleColor.DarkCyan);
 * 
 *             (String, Object)[] details = new (String, Object)[] 
 *             {
 *                 ("Active", true), ("Average", 12345.67M), ("Name", "Details")
 *             };
 * 
 *             IConsoleLogger logger = new ConsoleLogger(settings, facade);
 * 
 *             logger.Debug("This is a Debug logging entry.", details);
 *             logger.Trace("This is a Trace logging entry.", details);
 *             logger.Verbose("This is a Verbose logging entry.", details);
 *             logger.Message("This is a Message logging entry.", details);
 *             logger.Warning("This is a Warning logging entry.", details);
 *             logger.Error("This is a Error logging entry.", details);
 *             logger.Fatal("This is a Fatal logging entry.", details);
 *             logger.Critical("This is a Critical logging entry.", details);
 *             logger.Disaster("This is a Disaster logging entry.");
 *         }
 *     }
 * }
 */

/* An example of how to use standard console logger.
 * 
 * using Plexdata.LogWriter.Abstraction;
 * using Plexdata.LogWriter.Definitions;
 * using Plexdata.LogWriter.Extensions;
 * using Plexdata.LogWriter.Facades.Standard;
 * using Plexdata.LogWriter.Logging.Standard;
 * using Plexdata.LogWriter.Settings;
 * using System;
 * 
 * namespace Plexdata.LogWriter.Examples
 * {
 *     class Program
 *     {
 *         static void Main(String[] args)
 *         {
 *             IConsoleLoggerFacade facade = new ConsoleLoggerFacade();
 *             IConsoleLoggerSettings settings = new ConsoleLoggerSettings
 *             {
 *                 WindowTitle = "Console Logger Test Application",
 *                 UseColors = true,
 *                 PartSplit = '#',
 *                 LogLevel = LogLevel.Trace,
 *                 QuickEdit = true, // Does not have any effect in this context...
 *                 BufferSize = new Dimension(150, 1000),
 *             };
 * 
 *             settings.Coloring[LogLevel.Critical] = new Coloring(ConsoleColor.Yellow, ConsoleColor.DarkCyan);
 * 
 *             (String, Object)[] details = new (String, Object)[]
 *             {
 *                 ("Active", true), ("Average", 12345.67M), ("Name", "Details")
 *             };
 * 
 *             IConsoleLogger logger = new ConsoleLogger(settings, facade);
 * 
 *             logger.Debug("This is a Debug logging entry.", details);
 *             logger.Trace("This is a Trace logging entry.", details);
 *             logger.Verbose("This is a Verbose logging entry.", details);
 *             logger.Message("This is a Message logging entry.", details);
 *             logger.Warning("This is a Warning logging entry.", details);
 *             logger.Error("This is a Error logging entry.", details);
 *             logger.Fatal("This is a Fatal logging entry.", details);
 *             logger.Critical("This is a Critical logging entry.", details);
 *             logger.Disaster("This is a Disaster logging entry.");
 *         }
 *     }
 * }
 */

/* An example of how to use persistent logger.
 * 
 * using Plexdata.LogWriter.Abstraction;
 * using Plexdata.LogWriter.Definitions;
 * using Plexdata.LogWriter.Extensions;
 * using Plexdata.LogWriter.Logging;
 * using Plexdata.LogWriter.Settings;
 * using System;
 *
 * namespace Plexdata.LogWriter.Examples
 * {
 *     class Program
 *     {
 *         static void Main(String[] args)
 *         {
 *             IPersistentLoggerSettings settings = new PersistentLoggerSettings()
 *             {
 *                 Filename = @"c:\temp\logging\test.log",
 *                 LogLevel = LogLevel.Trace,
 *                 IsQueuing = false,
 *                 IsRolling = true,
 *                 Threshold = 3,
 *                 Encoding = Encoding.ASCII,
 *             };
 * 
 *             IPersistentLogger logger = new PersistentLogger(settings);
 * 
 *             logger.Debug("This is a Debug logging entry.");
 *             logger.Trace("This is a Trace logging entry.");
 *             logger.Verbose("This is a Verbose logging entry.");
 *             logger.Message("This is a Message logging entry.");
 *             logger.Warning("This is a Warning logging entry.");
 *             logger.Error("This is a Error logging entry.");
 *             logger.Fatal("This is a Fatal logging entry.");
 *             logger.Critical("This is a Critical logging entry.");
 *             logger.Disaster("This is a Disaster logging entry.");
 *         }
 *     }
 * }
 */

/* An example of how to use network logger together with GELF.
 * 
 * using Plexdata.LogWriter.Abstraction;
 * using Plexdata.LogWriter.Definitions;
 * using Plexdata.LogWriter.Logging;
 * using Plexdata.LogWriter.Settings;
 * using System;
 * 
 * namespace Plexdata.LogWriter.Examples
 * {
 *     class Program
 *     {
 *         static void Main(String[] args)
 *         {
 *             INetworkLoggerSettings settings = new NetworkLoggerSettings()
 *             {
 *                 LogLevel = LogLevel.Trace,
 *                 LogType = LogType.Gelf,
 *                 Host = "http://localhost/logging/gelf",
 *                 Port = 42031,
 *                 Address = Address.Unknown,
 *                 Protocol = Protocol.Web,
 *                 ShowKey = false,
 *                 ShowTime = true,
 *                 Termination = false,
 *                 Timeout = 100,
 *                 Method = "POST",
 *                 Content = "application/json"
 *             };
 * 
 *             using (INetworkLogger instance = new NetworkLogger(settings))
 *             {
 *                 instance.Write(LogLevel.Trace, "This is a Trace logging entry.");
 *                 instance.Write(LogLevel.Debug, "This is a Debug logging entry.");
 *                 instance.Write(LogLevel.Verbose, "This is a Verbose logging entry.");
 *                 instance.Write(LogLevel.Message, "This is a Message logging entry.");
 *                 instance.Write(LogLevel.Warning, "This is a Warning logging entry.");
 *                 instance.Write(LogLevel.Error, "This is a Error logging entry.");
 *                 instance.Write(LogLevel.Fatal, "This is a Fatal logging entry.");
 *                 instance.Write(LogLevel.Critical, "This is a Critical logging entry.");
 *                 instance.Write(LogLevel.Disaster, "This is a Disaster logging entry.");
 *             }
 *         }
 *     }
 * } 
 */

/* An example of how to use stream logger.
 * 
 * using Plexdata.LogWriter.Abstraction;
 * using Plexdata.LogWriter.Definitions;
 * using Plexdata.LogWriter.Extensions;
 * using Plexdata.LogWriter.Logging;
 * using Plexdata.LogWriter.Settings;
 * using System;
 * using System.IO;
 * using System.Text;
 * 
 * namespace Plexdata.LogWriter.Examples
 * {
 *     class Program
 *     {
 *         private class CustomStream : Stream
 *         {
 *             private readonly Encoding encoding = Encoding.Default;
 * 
 *             public CustomStream(Encoding encoding) { this.encoding = encoding; }
 * 
 *             public override Boolean CanRead { get { return false; } }
 * 
 *             public override Boolean CanSeek { get { return false; } }
 * 
 *             public override Boolean CanWrite { get { return true; } }
 * 
 *             public override Int64 Length { get { return 0; } }
 * 
 *             public override Int64 Position { get { return 0; } set { } }
 * 
 *             public override void Flush() { }
 * 
 *             public override Int32 Read(Byte[] buffer, Int32 offset, Int32 count) { return 0; }
 * 
 *             public override Int64 Seek(Int64 offset, SeekOrigin origin) { return 0; }
 * 
 *             public override void SetLength(Int64 value) { }
 * 
 *             public override void Write(Byte[] buffer, Int32 offset, Int32 count)
 *             {
 *                 // The real magic happens here...
 *                 Console.Write(this.encoding.GetChars(buffer), offset, count);
 *             }
 *         }
 * 
 *         static void Main(String[] args)
 *         {
 *             IStreamLoggerSettings settings = new StreamLoggerSettings
 *             {
 *                 LogLevel = LogLevel.Trace,
 *                 LogType = LogType.Raw,
 *                 Stream = new CustomStream(Encoding.ASCII),
 *                 Encoding = Encoding.ASCII
 *             };
 * 
 *             IStreamLogger logger = new StreamLogger(settings);
 * 
 *             logger.Debug("This is a Debug logging entry.");
 *             logger.Trace("This is a Trace logging entry.");
 *             logger.Verbose("This is a Verbose logging entry.");
 *             logger.Message("This is a Message logging entry.");
 *             logger.Warning("This is a Warning logging entry.");
 *             logger.Error("This is a Error logging entry.");
 *             logger.Fatal("This is a Fatal logging entry.");
 *             logger.Critical("This is a Critical logging entry.");
 *             logger.Disaster("This is a Disaster logging entry.");
 *         }
 *     }
 * }
 */

/* Example (1) of how to use composite logger.
 * 
 * using Plexdata.LogWriter.Abstraction;
 * using Plexdata.LogWriter.Extensions;
 * using Plexdata.LogWriter.Logging;
 * using System;
 * 
 * namespace Plexdata.LogWriter.Examples
 * {
 *     class Program
 *     {
 *         static void Main(String[] args)
 *         {
 *             ICompositeLogger logger = new CompositeLogger();
 * 
 *             // Assign one console logger and two persistent loggers with different files.
 *             logger.AddConsoleLogger()
 *                 .AddPersistentLogger(@"c:\log-files\log-file-1.log")
 *                 .AddPersistentLogger(@"c:\log-files\log-file-2.log");
 * 
 *             logger.Trace("Trace...");
 *             logger.Debug("Debug...");
 *             logger.Verbose("Verbose...");
 *             logger.Message("Message...");
 *             logger.Warning("Warning...");
 *             logger.Error("Error...");
 *             logger.Fatal("Fatal...");
 *             logger.Critical("Critical...");
 *             logger.Disaster("Disaster...");
 *         }
 *     }
 * }
 */

/* Example (2) of how to use composite logger.
 * 
 * using Plexdata.LogWriter.Abstraction;
 * using Plexdata.LogWriter.Extensions;
 * using Plexdata.LogWriter.Logging;
 * using Plexdata.LogWriter.Settings;
 * using System;
 * using System.IO;
 * 
 * namespace Plexdata.LogWriter.Examples
 * {
 *     class Program
 *     {
 *         static void Main(String[] args)
 *         {
 *             ICompositeLogger logger = new CompositeLogger();
 * 
 *             ILoggerSettingsBuilder builder = new LoggerSettingsBuilder();
 *             builder.SetFilename(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
 *             ILoggerSettingsSection loggerSettingsSection = builder.Build();
 * 
 *             // Assign one console logger and two persistent loggers with different files
 *             // but share values of same settings file between all loggers.
 *             logger.AddConsoleLogger(loggerSettingsSection)
 *                 .AddPersistentLogger(loggerSettingsSection, @"c:\log-files\log-file-1.log")
 *                 .AddPersistentLogger(loggerSettingsSection, @"c:\log-files\log-file-2.log");
 * 
 *             logger.Trace("Trace...");
 *             logger.Debug("Debug...");
 *             logger.Verbose("Verbose...");
 *             logger.Message("Message...");
 *             logger.Warning("Warning...");
 *             logger.Error("Error...");
 *             logger.Fatal("Fatal...");
 *             logger.Critical("Critical...");
 *             logger.Disaster("Disaster...");
 *         }
 *     }
 * }
 */

/* Example (3) of how to use composite logger.
 * 
 * using Plexdata.LogWriter.Abstraction;
 * using Plexdata.LogWriter.Definitions;
 * using Plexdata.LogWriter.Extensions;
 * using Plexdata.LogWriter.Logging;
 * using Plexdata.LogWriter.Settings;
 * using System;
 * 
 * namespace Plexdata.LogWriter.Examples
 * {
 *     class Program
 *     {
 *         static void Main(String[] args)
 *         {
 *             ICompositeLogger logger = new CompositeLogger();
 * 
 *             IConsoleLoggerSettings consoleLoggerSettings = new ConsoleLoggerSettings
 *             {
 *                 LogLevel = LogLevel.Trace,
 *                 WindowTitle = "Composite Console Logger Example"
 *             };
 * 
 *             IPersistentLoggerSettings persistentLoggerSettings1 = new PersistentLoggerSettings
 *             {
 *                 LogLevel = LogLevel.Debug,
 *                 Filename = @"c:\log-files\log-file-1.log"
 *             };
 * 
 *             IPersistentLoggerSettings persistentLoggerSettings2 = new PersistentLoggerSettings
 *             {
 *                 LogLevel = LogLevel.Verbose,
 *                 Filename = @"c:\log-files\log-file-2.log"
 *             };
 * 
 *             // Assign one console logger and two persistent loggers with different settings.
 *             logger.AddConsoleLogger(consoleLoggerSettings)
 *                 .AddPersistentLogger(persistentLoggerSettings1)
 *                 .AddPersistentLogger(persistentLoggerSettings2);
 * 
 *             logger.Trace("Trace...");
 *             logger.Debug("Debug...");
 *             logger.Verbose("Verbose...");
 *             logger.Message("Message...");
 *             logger.Warning("Warning...");
 *             logger.Error("Error...");
 *             logger.Fatal("Fatal...");
 *             logger.Critical("Critical...");
 *             logger.Disaster("Disaster...");
 *         }
 *     }
 * }
 */

/* Example (4) of how to use composite logger.
 * 
 * using Plexdata.LogWriter.Abstraction;
 * using Plexdata.LogWriter.Definitions;
 * using Plexdata.LogWriter.Extensions;
 * using Plexdata.LogWriter.Logging;
 * using Plexdata.LogWriter.Logging.Standard;
 * using Plexdata.LogWriter.Settings;
 * using System;
 * 
 * namespace Plexdata.LogWriter.Examples
 * {
 *     class Program
 *     {
 *         static void Main(String[] args)
 *         {
 *             ICompositeLogger<Program> logger = new CompositeLogger<Program>();
 * 
 *             IConsoleLoggerSettings consoleLoggerSettings = new ConsoleLoggerSettings
 *             {
 *                 LogLevel = LogLevel.Trace,
 *                 WindowTitle = "Composite Console Logger Example"
 *             };
 * 
 *             IPersistentLoggerSettings persistentLoggerSettings1 = new PersistentLoggerSettings
 *             {
 *                 LogLevel = LogLevel.Debug,
 *                 Filename = @"c:\log-files\log-file-1.log"
 *             };
 * 
 *             IPersistentLoggerSettings persistentLoggerSettings2 = new PersistentLoggerSettings
 *             {
 *                 LogLevel = LogLevel.Verbose,
 *                 Filename = @"c:\log-files\log-file-2.log"
 *             };
 * 
 *             IConsoleLogger<Program> consoleLogger = new ConsoleLogger<Program>(consoleLoggerSettings);
 *             IPersistentLogger<Program> persistentLogger1 = new PersistentLogger<Program>(persistentLoggerSettings1);
 *             IPersistentLogger<Program> persistentLogger2 = new PersistentLogger<Program>(persistentLoggerSettings2);
 * 
 *             // Assign one console logger and two persistent 
 *             // loggers, all of them with context relation.
 *             logger.AddConsoleLogger(consoleLogger)
 *                 .AddPersistentLogger(persistentLogger1)
 *                 .AddPersistentLogger(persistentLogger2);
 * 
 *             logger.Trace("Trace...");
 *             logger.Debug("Debug...");
 *             logger.Verbose("Verbose...");
 *             logger.Message("Message...");
 *             logger.Warning("Warning...");
 *             logger.Error("Error...");
 *             logger.Fatal("Fatal...");
 *             logger.Critical("Critical...");
 *             logger.Disaster("Disaster...");
 *         }
 *     }
 * }
 */

/* An example of how to use mail logger.
 * using Plexdata.LogWriter.Abstraction;
 * using Plexdata.LogWriter.Extensions;
 * using Plexdata.LogWriter.Logging;
 * using Plexdata.LogWriter.Settings;
 * using System;
 * using System.Collections.Generic;
 * using System.Text;
 * 
 * namespace Plexdata.LogWriter.Examples
 * {
 *     class Program
 *     {
 *         static void Main(String[] args)
 *         {
 *             IMailLoggerSettings settings = new MailLoggerSettings()
 *             {
 *                 Address = "from@example.org",
 *                 Username = "SMTP host username",
 *                 Password = "SMTP host password",
 *                 SmtpHost = "mail.example.org",
 *                 SmtpPort = 587,
 *                 UseSsl = true,
 *                 Encoding = Encoding.UTF8,
 *                 Subject = "Logger Mail",
 *                 Receivers = new List<String>() {
 *                     "to1@example.org",
 *                     "to2@example.org"
 *                 },
 *                 ClearCopies = new List<String>() {
 *                     "cc1@example.org",
 *                     "cc2@example.org"
 *                 },
 *                 BlindCopies = new List<String>() {
 *                     "bcc1@example.org",
 *                     "bcc2@example.org"
 *                 }
 *             };
 * 
 *             IMailLogger logger = new MailLogger(settings);
 * 
 *             (String, Object)[] details = new (String, Object)[]
 *             {
 *                 ("Active", true), ("Average", 12345.67M), ("Name", "Details")
 *             };
 * 
 *             logger.Debug("This is a Debug logging entry.", details);
 *             logger.Trace("This is a Trace logging entry.", details);
 *             logger.Verbose("This is a Verbose logging entry.", details);
 *             logger.Message("This is a Message logging entry.", details);
 *             logger.Warning("This is a Warning logging entry.", details);
 *             logger.Error("This is a Error logging entry.", details);
 *             logger.Fatal("This is a Fatal logging entry.", details);
 *             logger.Critical("This is a Critical logging entry.", details);
 *             logger.Disaster("This is a Disaster logging entry.", details);
 *         }
 *     }
 * }
 */
