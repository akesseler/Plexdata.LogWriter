/*
 * MIT License
 * 
 * Copyright (c) 2021 plexdata.de
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
using Plexdata.LogWriter.Extensions;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.LogWriter.Abstraction.Tests.Extensions
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [TestOf(nameof(LogLevelDisplayExtension))]
    public class LogLevelDisplayExtensionTests
    {
        #region Prologue

        [SetUp]
        [TearDown]
        public void SetupAndRestore()
        {
            foreach (LogLevel level in Enum.GetValues(typeof(LogLevel)))
            {
                level.RestoreDisplayText();
            }
        }

        #endregion

        #region ToDisplayText

        [TestCase(10, LogLevel.Disabled, "DISABLED")]
        [TestCase(11, LogLevel.Trace, "TRACE")]
        [TestCase(12, LogLevel.Debug, "DEBUG")]
        [TestCase(13, LogLevel.Verbose, "VERBOSE")]
        [TestCase(14, LogLevel.Message, "MESSAGE")]
        [TestCase(15, LogLevel.Warning, "WARNING")]
        [TestCase(16, LogLevel.Error, "ERROR")]
        [TestCase(17, LogLevel.Fatal, "FATAL")]
        [TestCase(18, LogLevel.Critical, "CRITICAL")]
        [TestCase(19, LogLevel.Default, "MESSAGE")]
        public void ToDisplayText_LogLevelWithDefaultValue_ResultIsDefaultDisplayText(Int32 index, LogLevel actual, String expected)
        {
            // NOTE: The index is only needed to ensure that "Default" is tested too!
            Assert.That(actual.ToDisplayText(), Is.EqualTo(expected));
        }

        [Test]
        public void ToDisplayText_WithUnknownLogLevel_ThrowsInvalidEnumArgumentException()
        {
            LogLevel actual = (LogLevel)42;
            Assert.That(() => actual.ToDisplayText(), Throws.InstanceOf<InvalidEnumArgumentException>());
        }

        #endregion

        #region RegisterDisplayText

        [TestCase(LogLevel.Disabled, null)]
        [TestCase(LogLevel.Disabled, "")]
        [TestCase(LogLevel.Disabled, " ")]
        [TestCase(LogLevel.Trace, null)]
        [TestCase(LogLevel.Trace, "")]
        [TestCase(LogLevel.Trace, " ")]
        [TestCase(LogLevel.Debug, null)]
        [TestCase(LogLevel.Debug, "")]
        [TestCase(LogLevel.Debug, " ")]
        [TestCase(LogLevel.Verbose, null)]
        [TestCase(LogLevel.Verbose, "")]
        [TestCase(LogLevel.Verbose, " ")]
        [TestCase(LogLevel.Message, null)]
        [TestCase(LogLevel.Message, "")]
        [TestCase(LogLevel.Message, " ")]
        [TestCase(LogLevel.Warning, null)]
        [TestCase(LogLevel.Warning, "")]
        [TestCase(LogLevel.Warning, " ")]
        [TestCase(LogLevel.Error, null)]
        [TestCase(LogLevel.Error, "")]
        [TestCase(LogLevel.Error, " ")]
        [TestCase(LogLevel.Fatal, null)]
        [TestCase(LogLevel.Fatal, "")]
        [TestCase(LogLevel.Fatal, " ")]
        [TestCase(LogLevel.Critical, null)]
        [TestCase(LogLevel.Critical, "")]
        [TestCase(LogLevel.Critical, " ")]
        public void RegisterDisplayText_LogLevelWithInvalidText_ThrowsArgumentOutOfRangeException(LogLevel actual, String expected)
        {
            Assert.That(() => actual.RegisterDisplayText(expected), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void RegisterDisplayText_UseUnknownLogLevel_ThrowsInvalidEnumArgumentException()
        {
            LogLevel actual = (LogLevel)42;
            Assert.That(() => actual.RegisterDisplayText("foo"), Throws.InstanceOf<InvalidEnumArgumentException>());
        }

        [TestCase(LogLevel.Disabled, "FooDisabled")]
        [TestCase(LogLevel.Disabled, "foodisabled")]
        [TestCase(LogLevel.Disabled, "FOODISABLED")]
        [TestCase(LogLevel.Trace, "FooTrace")]
        [TestCase(LogLevel.Trace, "footrace")]
        [TestCase(LogLevel.Trace, "FOOTRACE")]
        [TestCase(LogLevel.Debug, "FooDebug")]
        [TestCase(LogLevel.Debug, "foodebug")]
        [TestCase(LogLevel.Debug, "FOODEBUG")]
        [TestCase(LogLevel.Verbose, "FooVerbose")]
        [TestCase(LogLevel.Verbose, "fooverbose")]
        [TestCase(LogLevel.Verbose, "FOOVERBOSE")]
        [TestCase(LogLevel.Message, "FooMessage")]
        [TestCase(LogLevel.Message, "foomessage")]
        [TestCase(LogLevel.Message, "FOOMESSAGE")]
        [TestCase(LogLevel.Warning, "FooWarning")]
        [TestCase(LogLevel.Warning, "foowarning")]
        [TestCase(LogLevel.Warning, "FOOWARNING")]
        [TestCase(LogLevel.Error, "FooError")]
        [TestCase(LogLevel.Error, "fooerror")]
        [TestCase(LogLevel.Error, "FOOERROR")]
        [TestCase(LogLevel.Fatal, "FooFatal")]
        [TestCase(LogLevel.Fatal, "foofatal")]
        [TestCase(LogLevel.Fatal, "FOOFATAL")]
        [TestCase(LogLevel.Critical, "FooCritical")]
        [TestCase(LogLevel.Critical, "foocritical")]
        [TestCase(LogLevel.Critical, "FOOCRITICAL")]
        public void RegisterDisplayText_LogLevelWithValidText_ResultIsEqualToExpected(LogLevel actual, String expected)
        {
            actual.RegisterDisplayText(expected);

            Assert.That(actual.ToDisplayText(), Is.EqualTo(expected));
        }

        #endregion

        #region RestoreDisplayText

        [TestCase(LogLevel.Disabled, "FooDisabled", "DISABLED")]
        [TestCase(LogLevel.Trace, "FooTrace", "TRACE")]
        [TestCase(LogLevel.Debug, "FooDebug", "DEBUG")]
        [TestCase(LogLevel.Verbose, "FooVerbose", "VERBOSE")]
        [TestCase(LogLevel.Message, "FooMessage", "MESSAGE")]
        [TestCase(LogLevel.Warning, "FooWarning", "WARNING")]
        [TestCase(LogLevel.Error, "FooError", "ERROR")]
        [TestCase(LogLevel.Fatal, "FooFatal", "FATAL")]
        [TestCase(LogLevel.Critical, "FooCritical", "CRITICAL")]
        public void RestoreDisplayText_ChangeAndRestoreDisplayText_ResultIsEqualToExpected(LogLevel actual, String display, String expected)
        {
            actual.RegisterDisplayText(display);

            actual.RestoreDisplayText();

            Assert.That(actual.ToDisplayText(), Is.EqualTo(expected));
        }

        #endregion
    }
}
