/*
 * MIT License
 * 
 * Copyright (c) 2022 plexdata.de
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

namespace Plexdata.LogWriter.Logging.Standard
{
    /// <summary>
    /// The <see cref="Plexdata.LogWriter.Logging.Standard"/> namespace contains 
    /// all classes that represent the <i>Plexdata Console Logging Writers</i> for any 
    /// platform.
    /// </summary>
    /// <remarks>
    /// This namespace includes for example the classes <see cref="ConsoleLogger"/> 
    /// and <see cref="ConsoleLogger{TContext}"/>.
    /// </remarks>
    /// <example>
    /// See below for a fully qualified and executable example of how to use standard console logger.
    /// <code language="C#">
    /// using Plexdata.LogWriter.Abstraction;
    /// using Plexdata.LogWriter.Definitions;
    /// using Plexdata.LogWriter.Extensions;
    /// using Plexdata.LogWriter.Facades.Standard;
    /// using Plexdata.LogWriter.Logging.Standard;
    /// using Plexdata.LogWriter.Settings;
    /// using System;
    /// 
    /// namespace Plexdata.LogWriter.Examples
    /// {
    ///     class Program
    ///     {
    ///         static void Main(String[] args)
    ///         {
    ///             IConsoleLoggerFacade facade = new ConsoleLoggerFacade();
    ///             IConsoleLoggerSettings settings = new ConsoleLoggerSettings
    ///             {
    ///                 WindowTitle = "Console Logger Test Application",
    ///                 UseColors = true,
    ///                 PartSplit = '#',
    ///                 LogLevel = LogLevel.Trace,
    ///                 QuickEdit = true, // Does not have any effect in this context...
    ///                 BufferSize = new Dimension(150, 1000),
    ///             };
    /// 
    ///             settings.Coloring[LogLevel.Critical] = new Coloring(ConsoleColor.Yellow, ConsoleColor.DarkCyan);
    /// 
    ///             (String, Object)[] details = new (String, Object)[]
    ///             {
    ///                 ("Active", true), ("Average", 12345.67M), ("Name", "Details")
    ///             };
    /// 
    ///             IConsoleLogger logger = new ConsoleLogger(settings, facade);
    /// 
    ///             logger.Debug("This is a Debug logging entry.", details);
    ///             logger.Trace("This is a Trace logging entry.", details);
    ///             logger.Verbose("This is a Verbose logging entry.", details);
    ///             logger.Message("This is a Message logging entry.", details);
    ///             logger.Warning("This is a Warning logging entry.", details);
    ///             logger.Error("This is a Error logging entry.", details);
    ///             logger.Fatal("This is a Fatal logging entry.", details);
    ///             logger.Critical("This is a Critical logging entry.", details);
    ///             logger.Disaster("This is a Disaster logging entry.", details);
    /// 
    ///             Console.Write("Hit any key to finish... ");
    ///             Console.ReadKey();
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>
    [System.Runtime.CompilerServices.CompilerGenerated]
    class NamespaceDoc { }
}
