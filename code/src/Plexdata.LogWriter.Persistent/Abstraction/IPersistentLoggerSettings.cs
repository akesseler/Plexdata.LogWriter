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
using System.Text;

namespace Plexdata.LogWriter.Abstraction
{
    /// <summary>
    /// This interface represents the scope of settings used together with 
    /// the persistent logger.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The persistent logger settings extend the basic logger settings by 
    /// additional information that are only used in conjunction with the 
    /// persistent logger.
    /// </para>
    /// <para>
    /// The following overview provides some information about the rules that 
    /// are applied to the properties of this logger settings.
    /// </para>
    /// <list type="number">
    /// <item><description>
    /// If rolling is <b>off</b> and the threshold is equal to or less than 
    /// zero then only one logging file is written.
    /// </description></item>
    /// <item><description>
    /// If rolling is <b>off</b> and the threshold greater than zero then a set 
    /// of logging files are written, each of them with an appended time stamp.
    /// </description></item>
    /// <item><description>
    /// If rolling is <b>on</b> and the threshold is equal to or less than zero 
    /// then a default threshold of five megabytes is used and rolling is applied 
    /// with that default threshold.
    /// </description></item>
    /// <item><description>
    /// If rolling is <b>on</b> and the threshold is greater than zero then rolling 
    /// is applied with that threshold.
    /// </description></item>
    /// </list>
    /// <para>
    /// In case of rolling, a first logging file with suffix <c>_one</c> is created. 
    /// If that file reaches the threshold then the first file is closed and a second 
    /// file with suffix <c>_two</c> is created. If the second file reaches the threshold 
    /// then the seconf file is cloesed and the first file is reopened. In that case the 
    /// content of the fist file is completely discarded, and so on.
    /// </para>
    /// </remarks>
    public interface IPersistentLoggerSettings : ILoggerSettings
    {
        /// <summary>
        /// Gets or sets the fully qualified filename.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property allows to change the fully qualified name of the 
        /// assigned logging file.
        /// </para>
        /// <para>
        /// The value assigned to this property can also contain environment variables, 
        /// such as <c>%TMP%</c> for example. But note, no matter which platform is used, 
        /// each of the environment variables must be surrounded by a percentage character 
        /// (%). Otherwise, resolving environment variables may fail and ends up in an 
        /// exception.
        /// </para>
        /// </remarks>
        /// <example>
        /// Below find some examples of how to use environment variables in filenames.
        /// <code language="C#">
        /// settings.Filename = "%TMP%\\output.log";
        /// settings.Filename = "%TEMP%\\output.log";
        /// settings.Filename = "%LOCALAPPDATA%\\Temp\\output.log";
        /// settings.Filename = "%HOMEDRIVE%%HOMEPATH%\\AppData\\Local\\Temp\\output.log";
        /// </code>
        /// </example>
        /// <value>
        /// The fully qualified name of the logging file.
        /// </value>
        String Filename { get; set; }

        /// <summary>
        /// Enables or disabled message rolling.
        /// </summary>
        /// <remarks>
        /// This property allows enabling or disabling of rolling of the logging 
        /// files. Rolling means that the real logging file changes if a particular 
        /// condition is satisfied.
        /// </remarks>
        /// <value>
        /// True, rolling is enabled and false, rolling is disabled.
        /// </value>
        Boolean IsRolling { get; set; }

        /// <summary>
        /// Enables or disabled message queuing.
        /// </summary>
        /// <remarks>
        /// This property allows enabling or disabling of message queuing. This 
        /// means messages are written directly into the logging file if message 
        /// queuing is disabled. The other way round, message to write are cached 
        /// before they will be written.
        /// </remarks>
        /// <value>
        /// True, queuing is enabled and false, queuing is disabled.
        /// </value>
        Boolean IsQueuing { get; set; }

        /// <summary>
        /// The size in kilobyte at when a logging file should be switched.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The threshold is actually meant as approximate value. This in turn 
        /// means that the file size is determined after a logging message has 
        /// been written. 
        /// </para>
        /// <para>
        /// The threshold of less than or equal to zero means no limitation.
        /// </para>
        /// </remarks>
        /// <value>
        /// The threshold of the file size in kilobyte.
        /// </value>
        Int32 Threshold { get; set; }

        /// <summary>
        /// Gets or sets the used file encoding.
        /// </summary>
        /// <remarks>
        /// This property allows to change the file encoding to be used.
        /// </remarks>
        /// <value>
        /// The used file encoding.
        /// </value>
        Encoding Encoding { get; set; }
    }
}
