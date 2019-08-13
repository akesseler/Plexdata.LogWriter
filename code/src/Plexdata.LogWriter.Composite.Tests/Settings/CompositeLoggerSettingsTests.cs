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

using Moq;
using NUnit.Framework;
using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Settings;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.LogWriter.Stream.Tests.Settings
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [TestOf(nameof(CompositeLoggerSettings))]
    public class CompositeLoggerSettingsTests
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
        }

        #endregion

        #region Construction

        [Test]
        public void CompositeLoggerSettings_DefaultConstruction_GetSectionCalledNever()
        {
            CompositeLoggerSettings instance = new CompositeLoggerSettings();

            this.configuration.Verify(x => x.GetSection("Plexdata:LogWriter:Settings"), Times.Never);
        }

        [Test]
        public void CompositeLoggerSettings_ExtendedConstructionConfigurationInvalid_GetSectionCalledNever()
        {
            CompositeLoggerSettings instance = new CompositeLoggerSettings((ILoggerSettingsSection)null);

            this.configuration.Verify(x => x.GetSection("Plexdata:LogWriter:Settings"), Times.Never);
        }

        [Test]
        public void CompositeLoggerSettings_ExtendedConstructionConfigurationValid_GetSectionCalledOnce()
        {
            CompositeLoggerSettings instance = new CompositeLoggerSettings(this.configuration.Object);

            this.configuration.Verify(x => x.GetSection("Plexdata:LogWriter:Settings"), Times.Once);
        }

        #endregion
    }
}


