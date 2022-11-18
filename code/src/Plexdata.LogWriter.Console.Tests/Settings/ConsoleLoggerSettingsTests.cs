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

using Moq;
using NUnit.Framework;
using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Settings;
using Plexdata.Utilities.Testing;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.LogWriter.Console.Tests.Settings
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.UnitTest)]
    [TestOf(nameof(ConsoleLoggerSettings))]
    public class ConsoleLoggerSettingsTests
    {
        #region Prologue

        private Mock<ILoggerSettingsSection> section;
        private Mock<ILoggerSettingsSection> configuration;

        [SetUp]
        public void Setup()
        {
            this.section = new Mock<ILoggerSettingsSection>();
            this.configuration = new Mock<ILoggerSettingsSection>();

            this.configuration
                .Setup(x => x.GetSection(It.IsAny<String>()))
                .Returns(this.section.Object);

            this.section
                .Setup(x => x.GetSection(It.IsAny<String>()))
                .Returns(this.section.Object);
        }

        #endregion

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
            Assert.That(instance.Coloring.Count, Is.EqualTo(9));
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
            Assert.That(instance.Coloring[LogLevel.Disaster].Foreground, Is.EqualTo(ConsoleColor.Black));
            Assert.That(instance.Coloring[LogLevel.Disaster].Background, Is.EqualTo(ConsoleColor.Red));
        }

        [Test]
        public void ConsoleLoggerSettings_ValidateDefaultSettingsForInvalidConfiguration_DefaultSettingsAsExpected()
        {
            ConsoleLoggerSettings instance = new ConsoleLoggerSettings(null);

            Assert.That(instance.UseColors, Is.True);
            Assert.That(instance.WindowTitle, Is.Empty);
            Assert.That(instance.QuickEdit, Is.False);
            Assert.That(instance.BufferSize.IsValid, Is.False);
            Assert.That(instance.BufferSize.Width, Is.Zero);
            Assert.That(instance.BufferSize.Lines, Is.Zero);
            Assert.That(instance.Coloring.Count, Is.EqualTo(9));
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
            Assert.That(instance.Coloring[LogLevel.Disaster].Foreground, Is.EqualTo(ConsoleColor.Black));
            Assert.That(instance.Coloring[LogLevel.Disaster].Background, Is.EqualTo(ConsoleColor.Red));
        }

        [Test]
        public void ConsoleLoggerSettings_ConfigurationValid_GetSectionCalledAsExpected()
        {
            ConsoleLoggerSettings instance = new ConsoleLoggerSettings(this.configuration.Object);

            this.configuration.Verify(x => x.GetSection("Plexdata:LogWriter:Settings"), Times.Exactly(2));
            this.section.Verify(x => x.GetSection("BufferSize"), Times.Once);
            this.section.Verify(x => x.GetSection("Coloring"), Times.Once);
        }

        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase(" ", true)]
        [TestCase("invalid", true)]
        [TestCase("true", true)]
        [TestCase("TRUE", true)]
        [TestCase("True", true)]
        [TestCase("false", false)]
        [TestCase("FALSE", false)]
        [TestCase("False", false)]
        public void ConsoleLoggerSettings_ConfigurationValid_GetSectionValueForUseColorsAsExpected(String value, Object expected)
        {
            this.section.SetupGet(x => x["UseColors"]).Returns(value);

            ConsoleLoggerSettings instance = new ConsoleLoggerSettings(this.configuration.Object);

            Assert.That(instance.UseColors, Is.EqualTo(expected));
        }

        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase("WindowTitle", "WindowTitle")]
        [TestCase("Window Title", "Window Title")]
        [TestCase("WINDOWTITLE", "WINDOWTITLE")]
        [TestCase("WINDOW TITLE", "WINDOW TITLE")]
        public void ConsoleLoggerSettings_ConfigurationValid_GetSectionValueForWindowTitleAsExpected(String value, Object expected)
        {
            this.section.SetupGet(x => x["WindowTitle"]).Returns(value);

            ConsoleLoggerSettings instance = new ConsoleLoggerSettings(this.configuration.Object);

            Assert.That(instance.WindowTitle, Is.EqualTo(expected));
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("invalid", false)]
        [TestCase("true", true)]
        [TestCase("TRUE", true)]
        [TestCase("True", true)]
        [TestCase("false", false)]
        [TestCase("FALSE", false)]
        [TestCase("False", false)]
        public void ConsoleLoggerSettings_ConfigurationValid_GetSectionValueForQuickEditAsExpected(String value, Object expected)
        {
            this.section.SetupGet(x => x["QuickEdit"]).Returns(value);

            ConsoleLoggerSettings instance = new ConsoleLoggerSettings(this.configuration.Object);

            Assert.That(instance.QuickEdit, Is.EqualTo(expected));
        }

        [TestCase(null, null, 0, 0)]
        [TestCase("", "", 0, 0)]
        [TestCase(" ", " ", 0, 0)]
        [TestCase("invalid", "invalid", 0, 0)]
        [TestCase(null, "23", 0, 0)]
        [TestCase("", "23", 0, 0)]
        [TestCase(" ", "23", 0, 0)]
        [TestCase("invalid", "23", 0, 0)]
        [TestCase("42", null, 0, 0)]
        [TestCase("42", "", 0, 0)]
        [TestCase("42", " ", 0, 0)]
        [TestCase("42", "invalid", 0, 0)]
        [TestCase("0", "0", 0, 0)]
        [TestCase("42", "23", 42, 23)]
        [TestCase("42.0", "23.5", 0, 0)]
        public void ConsoleLoggerSettings_ConfigurationValid_GetSectionValueForBufferSizeAsExpected(String sWidth, String sLines, Int32 nWidth, Int32 nLines)
        {
            Mock<ILoggerSettingsSection> dimension = new Mock<ILoggerSettingsSection>();

            this.section.Setup(x => x.GetSection("BufferSize")).Returns(dimension.Object);

            dimension.SetupGet(x => x["Width"]).Returns(sWidth);
            dimension.SetupGet(x => x["Lines"]).Returns(sLines);

            ConsoleLoggerSettings instance = new ConsoleLoggerSettings(this.configuration.Object);

            Assert.That(instance.BufferSize.Width, Is.EqualTo(nWidth));
            Assert.That(instance.BufferSize.Lines, Is.EqualTo(nLines));
        }

        [TestCase(LogLevel.Trace, null, null, ConsoleColor.Gray, ConsoleColor.Black)]
        [TestCase(LogLevel.Trace, "", "", ConsoleColor.Gray, ConsoleColor.Black)]
        [TestCase(LogLevel.Trace, " ", " ", ConsoleColor.Gray, ConsoleColor.Black)]
        [TestCase(LogLevel.Trace, "red", null, ConsoleColor.Red, ConsoleColor.Black)]
        [TestCase(LogLevel.Trace, "red", "", ConsoleColor.Red, ConsoleColor.Black)]
        [TestCase(LogLevel.Trace, "red", " ", ConsoleColor.Red, ConsoleColor.Black)]
        [TestCase(LogLevel.Trace, "RED", null, ConsoleColor.Red, ConsoleColor.Black)]
        [TestCase(LogLevel.Trace, "RED", "", ConsoleColor.Red, ConsoleColor.Black)]
        [TestCase(LogLevel.Trace, "RED", " ", ConsoleColor.Red, ConsoleColor.Black)]
        [TestCase(LogLevel.Trace, "Red", null, ConsoleColor.Red, ConsoleColor.Black)]
        [TestCase(LogLevel.Trace, "Red", "", ConsoleColor.Red, ConsoleColor.Black)]
        [TestCase(LogLevel.Trace, "Red", " ", ConsoleColor.Red, ConsoleColor.Black)]
        [TestCase(LogLevel.Trace, null, "green", ConsoleColor.Gray, ConsoleColor.Green)]
        [TestCase(LogLevel.Trace, "", "green", ConsoleColor.Gray, ConsoleColor.Green)]
        [TestCase(LogLevel.Trace, " ", "green", ConsoleColor.Gray, ConsoleColor.Green)]
        [TestCase(LogLevel.Trace, null, "GREEN", ConsoleColor.Gray, ConsoleColor.Green)]
        [TestCase(LogLevel.Trace, "", "GREEN", ConsoleColor.Gray, ConsoleColor.Green)]
        [TestCase(LogLevel.Trace, " ", "GREEN", ConsoleColor.Gray, ConsoleColor.Green)]
        [TestCase(LogLevel.Trace, null, "Green", ConsoleColor.Gray, ConsoleColor.Green)]
        [TestCase(LogLevel.Trace, "", "Green", ConsoleColor.Gray, ConsoleColor.Green)]
        [TestCase(LogLevel.Trace, " ", "Green", ConsoleColor.Gray, ConsoleColor.Green)]
        [TestCase(LogLevel.Trace, "red", "green", ConsoleColor.Red, ConsoleColor.Green)]
        [TestCase(LogLevel.Trace, "RED", "GREEN", ConsoleColor.Red, ConsoleColor.Green)]
        [TestCase(LogLevel.Trace, "Red", "Green", ConsoleColor.Red, ConsoleColor.Green)]
        [TestCase(LogLevel.Warning, null, null, ConsoleColor.Yellow, ConsoleColor.Black)]
        [TestCase(LogLevel.Warning, "", "", ConsoleColor.Yellow, ConsoleColor.Black)]
        [TestCase(LogLevel.Warning, " ", " ", ConsoleColor.Yellow, ConsoleColor.Black)]
        [TestCase(LogLevel.Warning, "red", null, ConsoleColor.Red, ConsoleColor.Black)]
        [TestCase(LogLevel.Warning, "red", "", ConsoleColor.Red, ConsoleColor.Black)]
        [TestCase(LogLevel.Warning, "red", " ", ConsoleColor.Red, ConsoleColor.Black)]
        [TestCase(LogLevel.Warning, "RED", null, ConsoleColor.Red, ConsoleColor.Black)]
        [TestCase(LogLevel.Warning, "RED", "", ConsoleColor.Red, ConsoleColor.Black)]
        [TestCase(LogLevel.Warning, "RED", " ", ConsoleColor.Red, ConsoleColor.Black)]
        [TestCase(LogLevel.Warning, "Red", null, ConsoleColor.Red, ConsoleColor.Black)]
        [TestCase(LogLevel.Warning, "Red", "", ConsoleColor.Red, ConsoleColor.Black)]
        [TestCase(LogLevel.Warning, "Red", " ", ConsoleColor.Red, ConsoleColor.Black)]
        [TestCase(LogLevel.Warning, null, "green", ConsoleColor.Yellow, ConsoleColor.Green)]
        [TestCase(LogLevel.Warning, "", "green", ConsoleColor.Yellow, ConsoleColor.Green)]
        [TestCase(LogLevel.Warning, " ", "green", ConsoleColor.Yellow, ConsoleColor.Green)]
        [TestCase(LogLevel.Warning, null, "GREEN", ConsoleColor.Yellow, ConsoleColor.Green)]
        [TestCase(LogLevel.Warning, "", "GREEN", ConsoleColor.Yellow, ConsoleColor.Green)]
        [TestCase(LogLevel.Warning, " ", "GREEN", ConsoleColor.Yellow, ConsoleColor.Green)]
        [TestCase(LogLevel.Warning, null, "Green", ConsoleColor.Yellow, ConsoleColor.Green)]
        [TestCase(LogLevel.Warning, "", "Green", ConsoleColor.Yellow, ConsoleColor.Green)]
        [TestCase(LogLevel.Warning, " ", "Green", ConsoleColor.Yellow, ConsoleColor.Green)]
        [TestCase(LogLevel.Warning, "red", "green", ConsoleColor.Red, ConsoleColor.Green)]
        [TestCase(LogLevel.Warning, "RED", "GREEN", ConsoleColor.Red, ConsoleColor.Green)]
        [TestCase(LogLevel.Warning, "Red", "Green", ConsoleColor.Red, ConsoleColor.Green)]
        public void ConsoleLoggerSettings_ConfigurationValid_GetSectionValueForColoringAsExpected(LogLevel level, String sForeground, String sBackground, ConsoleColor nForeground, ConsoleColor nBackground)
        {
            Mock<ILoggerSettingsSection> parent = new Mock<ILoggerSettingsSection>();
            Mock<ILoggerSettingsSection> coloring = new Mock<ILoggerSettingsSection>();
            Mock<ILoggerSettingsSection> others = new Mock<ILoggerSettingsSection>();

            this.section.Setup(x => x.GetSection("Coloring")).Returns(parent.Object);
            parent.Setup(x => x.GetSection(It.IsAny<String>())).Returns((String current) => { return (current == level.ToString()) ? coloring.Object : others.Object; });

            coloring.SetupGet(x => x["Foreground"]).Returns(sForeground);
            coloring.SetupGet(x => x["Background"]).Returns(sBackground);

            ConsoleLoggerSettings instance = new ConsoleLoggerSettings(this.configuration.Object);

            Assert.That(instance.Coloring[level].Foreground, Is.EqualTo(nForeground));
            Assert.That(instance.Coloring[level].Background, Is.EqualTo(nBackground));
        }

        #endregion

        #region UseColors

        [Test]
        public void UseColors_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            ConsoleLoggerSettings instance = new ConsoleLoggerSettings();

            instance.UseColors = false;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.UseColors = true;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void UseColors_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            ConsoleLoggerSettings instance = new ConsoleLoggerSettings();

            instance.UseColors = true;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.UseColors = true;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region WindowTitle

        [Test]
        public void WindowTitle_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            ConsoleLoggerSettings instance = new ConsoleLoggerSettings();

            instance.WindowTitle = "old window title";
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.WindowTitle = "new window title";

            Assert.That(fired, Is.True);
        }

        [Test]
        public void WindowTitle_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            ConsoleLoggerSettings instance = new ConsoleLoggerSettings();

            instance.WindowTitle = "same window title";
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.WindowTitle = "same window title";

            Assert.That(fired, Is.False);
        }

        #endregion

        #region QuickEdit

        [Test]
        public void QuickEdit_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            ConsoleLoggerSettings instance = new ConsoleLoggerSettings();

            instance.QuickEdit = false;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.QuickEdit = true;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void QuickEdit_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            ConsoleLoggerSettings instance = new ConsoleLoggerSettings();

            instance.QuickEdit = true;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.QuickEdit = true;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region BufferSize

        [Test]
        public void BufferSize_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            ConsoleLoggerSettings instance = new ConsoleLoggerSettings();

            instance.BufferSize = new Dimension();
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.BufferSize = new Dimension(42, 23);

            Assert.That(fired, Is.True);
        }

        [Test]
        public void BufferSize_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            ConsoleLoggerSettings instance = new ConsoleLoggerSettings();

            instance.BufferSize = new Dimension(66, 6);
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.BufferSize = new Dimension(66, 6);

            Assert.That(fired, Is.False);
        }

        #endregion

        #region Coloring

        [Test]
        public void Coloring_ValueChanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            ConsoleLoggerSettings instance = new ConsoleLoggerSettings();

            instance.Coloring[LogLevel.Critical] = new Coloring(ConsoleColor.Cyan, ConsoleColor.Blue);
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Coloring[LogLevel.Critical] = new Coloring(ConsoleColor.Blue, ConsoleColor.Cyan);

            Assert.That(fired, Is.False);
        }

        [Test]
        public void Coloring_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            ConsoleLoggerSettings instance = new ConsoleLoggerSettings();

            instance.Coloring[LogLevel.Critical] = new Coloring(ConsoleColor.Cyan, ConsoleColor.Blue);
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Coloring[LogLevel.Critical] = new Coloring(ConsoleColor.Cyan, ConsoleColor.Blue);

            Assert.That(fired, Is.False);
        }

        #endregion
    }
}
