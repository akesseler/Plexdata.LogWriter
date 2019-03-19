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
using System.ComponentModel;

namespace Plexdata.LogWriter.Definitions
{
    /// <summary>
    /// This class represents the assignment of foreground and background colors.
    /// </summary>
    /// <remarks>
    /// The coloring assignments are used to configure a combination of foreground 
    /// and background color to be used together with a particular message type.
    /// </remarks>
    public class Coloring
    {
        #region Public fields

        /// <summary>
        /// The default foreground color to be used.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="ConsoleColor.Gray"/> is used as 
        /// default foreground color. 
        /// </remarks>
        public const ConsoleColor DefaultForeground = ConsoleColor.Gray;

        /// <summary>
        /// The default background color to be used.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="ConsoleColor.Black"/> is used as 
        /// default foreground color. 
        /// </remarks>
        public const ConsoleColor DefaultBackground = ConsoleColor.Black;

        #endregion

        #region Construction

        /// <summary>
        /// The default constructor initializes all properties with its default 
        /// values.
        /// </summary>
        /// <remarks>
        /// Task of the default constructor is the setup of all properties with its 
        /// default values. These default values are <see cref="Coloring.DefaultForeground"/> 
        /// for the <see cref="Foreground"/> and <see cref="Coloring.DefaultBackground"/> for 
        /// the <see cref="Background"/>. 
        /// </remarks>
        /// <seealso cref="Coloring(ConsoleColor, ConsoleColor)"/>
        public Coloring()
            : this(Coloring.DefaultForeground, Coloring.DefaultBackground)
        {
        }

        /// <summary>
        /// The parameterized constructor initializes the properties with provided 
        /// values.
        /// </summary>
        /// Task of this constructor is the configuration of the foreground and 
        /// background color with provided values.
        /// <remarks>
        /// Task of this constructor is to initialize the properties with provided 
        /// parameter values.
        /// </remarks>
        /// <param name="foreground">
        /// The color used as foreground color.
        /// </param>
        /// <param name="background">
        /// The color used as background color.
        /// </param>
        /// <exception cref="InvalidEnumArgumentException">
        /// This exception is thrown as soon as provided <paramref name="foreground"/> 
        /// or provided <paramref name="background"/> is not defined in enumeration 
        /// <see cref="ConsoleColor"/>.
        /// </exception>
        public Coloring(ConsoleColor foreground, ConsoleColor background)
            : base()
        {
            if (!Enum.IsDefined(typeof(ConsoleColor), foreground))
            {
                throw new InvalidEnumArgumentException(nameof(foreground), (Int32)foreground, typeof(ConsoleColor));
            }

            if (!Enum.IsDefined(typeof(ConsoleColor), background))
            {
                throw new InvalidEnumArgumentException(nameof(background), (Int32)background, typeof(ConsoleColor));
            }

            this.Foreground = foreground;
            this.Background = background;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the assigned foreground color.
        /// </summary>
        /// <remarks>
        /// The foreground color can only be assigned in the 
        /// class constructor.
        /// </remarks>
        /// <value>
        /// The assigned foreground color.
        /// </value>
        /// <seealso cref="Coloring(ConsoleColor, ConsoleColor)"/>
        public ConsoleColor Foreground { get; private set; }

        /// <summary>
        /// Gets the assigned background color.
        /// </summary>
        /// <remarks>
        /// The background color can only be assigned in the 
        /// class constructor.
        /// </remarks>
        /// <value>
        /// The assigned background color.
        /// </value>
        /// <seealso cref="Coloring(ConsoleColor, ConsoleColor)"/>
        public ConsoleColor Background { get; private set; }

        #endregion

        #region Public methods

        /// <inheritdoc />
        /// <remarks>
        /// This method has been overwritten by this class.
        /// </remarks>
        public override String ToString()
        {
            return $"{nameof(this.Foreground)}={this.Foreground}, {nameof(this.Background)}={this.Background}";
        }

        #endregion
    }
}
