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

using Microsoft.Extensions.DependencyInjection;
using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Extensions;
using Plexdata.LogWriter.Logging;
using Plexdata.LogWriter.Logging.Standard;
using Plexdata.LogWriter.Settings;
using System;
using System.IO;

namespace Plexdata.LogWriter.Testing.Helper.Net.Core
{
    class Program
    {
        static void Main(String[] args)
        {
            Program program = new Program();
            program.Run();
        }

        private void Run()
        {
            Boolean terminate = false;

            while (!terminate)
            {
                switch (this.GetSelection())
                {
                    case 0:
                        terminate = true;
                        break;
                    case 1:
                        this.ConsoleLoggerWithJsonConfiguration();
                        break;
                    case 2:
                        this.ConsoleLoggerWithXmlConfiguration();
                        break;
                    case 3:
                        this.PersistentLoggerWithJsonConfiguration();
                        break;
                    case 4:
                        this.PersistentLoggerWithXmlConfiguration();
                        break;
                }
            }
        }

        private Int32 GetSelection(String message = null)
        {
            Console.Clear();

            Console.WriteLine("Choose one of the sections below.");
            Console.WriteLine("");
            Console.WriteLine("  1  -  Run Console Logger with JSON configuration.");
            Console.WriteLine("  2  -  Run Console Logger with XML configuration.");
            Console.WriteLine("  3  -  Run Persistent Logger with JSON configuration.");
            Console.WriteLine("  4  -  Run Persistent Logger with XML configuration.");
            Console.WriteLine("  Q  -  Quit application.");
            Console.WriteLine("");
            if (!String.IsNullOrWhiteSpace(message)) { Console.WriteLine(message); }

            switch (Console.ReadKey(true).KeyChar.ToString().ToLower())
            {
                case "1":
                    return 1;
                case "2":
                    return 2;
                case "3":
                    return 3;
                case "4":
                    return 4;
                case "q":
                    return 0;
                default:
                    return this.GetSelection("Try again...");
            }
        }

        private void ConsoleLoggerWithJsonConfiguration()
        {
            Console.Clear();
            Console.WriteLine("Console Logger with JSON Configuration.");

            try
            {
                ILoggerSettingsBuilder builder = new LoggerSettingsBuilder();
                builder.SetFilename("console-logger-settings.json");

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

                Console.WriteLine("Accomplished...");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            Console.WriteLine("Hit any key to continue...");
            Console.ReadKey(true);
        }

        private void ConsoleLoggerWithXmlConfiguration()
        {
            Console.Clear();
            Console.WriteLine("Console Logger with XML Configuration.");

            try
            {
                ILoggerSettingsBuilder builder = new LoggerSettingsBuilder();
                builder.SetFilename("console-logger-settings.xml");

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

                Console.WriteLine("Accomplished...");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            Console.WriteLine("Hit any key to continue...");
            Console.ReadKey(true);
        }

        private void PersistentLoggerWithJsonConfiguration()
        {
            Console.Clear();
            Console.WriteLine("Persistent Logger with JSON Configuration.");

            try
            {
                ILoggerSettingsBuilder builder = new LoggerSettingsBuilder();
                builder.SetFilename("persistent-logger-settings.json");

                ILoggerSettingsSection config = builder.Build();
                IServiceCollection services = new ServiceCollection();

                services.AddSingleton<ILoggerSettingsSection>(config);
                services.AddSingleton<IPersistentLogger, PersistentLogger>();
                services.AddSingleton<IPersistentLoggerSettings, PersistentLoggerSettings>();

                IServiceProvider provider = services.BuildServiceProvider();
                IPersistentLogger logger = provider.GetService<IPersistentLogger>();

                logger.Trace("This is a Trace logging entry.");
                logger.Debug("This is a Debug logging entry.");
                logger.Verbose("This is a Verbose logging entry.");
                logger.Message("This is a Message logging entry.");
                logger.Warning("This is a Warning logging entry.");
                logger.Error("This is a Error logging entry.");
                logger.Fatal("This is a Fatal logging entry.");
                logger.Critical("This is a Critical logging entry.");

                Console.WriteLine("Accomplished...");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            Console.WriteLine("Hit any key to continue...");
            Console.ReadKey(true);
        }

        private void PersistentLoggerWithXmlConfiguration()
        {
            Console.Clear();
            Console.WriteLine("Persistent Logger with XML Configuration.");

            try
            {
                ILoggerSettingsBuilder builder = new LoggerSettingsBuilder();
                builder.SetFilename("persistent-logger-settings.xml");

                ILoggerSettingsSection config = builder.Build();
                IServiceCollection services = new ServiceCollection();

                services.AddSingleton<ILoggerSettingsSection>(config);
                services.AddSingleton<IPersistentLogger, PersistentLogger>();
                services.AddSingleton<IPersistentLoggerSettings, PersistentLoggerSettings>();

                IServiceProvider provider = services.BuildServiceProvider();
                IPersistentLogger logger = provider.GetService<IPersistentLogger>();

                logger.Trace("This is a Trace logging entry.");
                logger.Debug("This is a Debug logging entry.");
                logger.Verbose("This is a Verbose logging entry.");
                logger.Message("This is a Message logging entry.");
                logger.Warning("This is a Warning logging entry.");
                logger.Error("This is a Error logging entry.");
                logger.Fatal("This is a Fatal logging entry.");
                logger.Critical("This is a Critical logging entry.");

                Console.WriteLine("Accomplished...");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            Console.WriteLine("Hit any key to continue...");
            Console.ReadKey(true);
        }
    }
}
