/*
 * MIT License
 * 
 * Copyright (c) 2019 plexdata.de
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
 * 
 *             Console.Write("Hit any key to finish... ");
 *             Console.ReadKey();
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
 * 
 *             Console.Write("Hit any key to finish... ");
 *             Console.ReadKey();
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
 * 
 *             Console.Write("Hit any key to finish... ");
 *             Console.ReadKey();
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
 * 
 *             Console.Write("Hit any key to finish... ");
 *             Console.ReadKey();
 *         }
 *     }
 * }
 */
