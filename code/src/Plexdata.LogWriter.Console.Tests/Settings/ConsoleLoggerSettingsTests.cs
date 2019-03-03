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

using NUnit.Framework;
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Settings.Console;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.LogWriter.Console.Tests.Settings
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [TestOf(nameof(ConsoleLoggerSettings))]
    public class ConsoleLoggerSettingsTests
    {
        #region ConsoleLoggerSettings

        [Test]
        public void ConsoleLoggerSettings_ValidateDefaultSettings_DefaultSettingsAsExpected()
        {
            ConsoleLoggerSettings instance = new ConsoleLoggerSettings();

            Assert.That(instance.UseColors, Is.True);
            Assert.That(instance.WindowTitle, Is.Empty);
            Assert.That(instance.QuickEdit, Is.False);
            Assert.That(instance.BufferSize.IsValid, Is.False);
            Assert.That(instance.BufferSize.Width, Is.Zero);
            Assert.That(instance.BufferSize.Lines, Is.Zero);
            Assert.That(instance.Coloring.Count, Is.EqualTo(8));
            Assert.That(instance.Coloring[LogLevel.Trace].Foreground, Is.EqualTo(ConsoleColor.Gray));
            Assert.That(instance.Coloring[LogLevel.Trace].Background, Is.EqualTo(ConsoleColor.Black));
            Assert.That(instance.Coloring[LogLevel.Debug].Foreground, Is.EqualTo(ConsoleColor.Gray));
            Assert.That(instance.Coloring[LogLevel.Debug].Background, Is.EqualTo(ConsoleColor.Black));
            Assert.That(instance.Coloring[LogLevel.Verbose].Foreground, Is.EqualTo(ConsoleColor.White));
            Assert.That(instance.Coloring[LogLevel.Verbose].Background, Is.EqualTo(ConsoleColor.Black));
            Assert.That(instance.Coloring[LogLevel.Message].Foreground, Is.EqualTo(ConsoleColor.White));
            Assert.That(instance.Coloring[LogLevel.Message].Background, Is.EqualTo(ConsoleColor.Black));
            Assert.That(instance.Coloring[LogLevel.Warning].Foreground, Is.EqualTo(ConsoleColor.Yellow));
            Assert.That(instance.Coloring[LogLevel.Warning].Background, Is.EqualTo(ConsoleColor.Black));
            Assert.That(instance.Coloring[LogLevel.Error].Foreground, Is.EqualTo(ConsoleColor.Red));
            Assert.That(instance.Coloring[LogLevel.Error].Background, Is.EqualTo(ConsoleColor.Black));
            Assert.That(instance.Coloring[LogLevel.Fatal].Foreground, Is.EqualTo(ConsoleColor.Gray));
            Assert.That(instance.Coloring[LogLevel.Fatal].Background, Is.EqualTo(ConsoleColor.DarkRed));
            Assert.That(instance.Coloring[LogLevel.Critical].Foreground, Is.EqualTo(ConsoleColor.Black));
            Assert.That(instance.Coloring[LogLevel.Critical].Background, Is.EqualTo(ConsoleColor.Red));
        }

        #endregion
    }
}
