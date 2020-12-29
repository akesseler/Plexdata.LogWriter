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

namespace Plexdata.LogWriter.Definitions
{
    /// <summary>
    /// This class represents the dimension of a console window.
    /// </summary>
    /// <remarks>
    /// The dimension of a console window is characterized by a number of lines 
    /// and a number of columns. Against this background, the total number of 
    /// possible text can be calculated by a multiplication of the width and 
    /// the number of lines.
    /// </remarks>
    public class Dimension
    {
        #region Construction

        /// <summary>
        /// The default constructor initializes all properties with its default 
        /// values.
        /// </summary>
        /// <remarks>
        /// Task of the default constructor is the setup of all properties with its 
        /// default values. These default values are <c>0</c> for the <see cref="Width"/> 
        /// and <c>0</c> for the <see cref="Lines"/>. 
        /// </remarks>
        /// <seealso cref="Dimension(Int32, Int32)"/>
        public Dimension()
            : this(0, 0)
        {
        }

        /// <summary>
        /// The parameterized constructor initializes the properties with provided 
        /// values.
        /// </summary>
        /// Task of this constructor is the configuration of the width and lines with 
        /// provided values.
        /// <remarks>
        /// Task of this constructor is to initialize the properties with provided 
        /// parameter values.
        /// </remarks>
        /// <param name="width">
        /// The number of characters of the console buffer.
        /// </param>
        /// <param name="lines">
        /// The number of lines of the console buffer.
        /// </param>
        public Dimension(Int32 width, Int32 lines)
            : base()
        {
            this.Width = width;
            this.Lines = lines;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Determines if this instance is a valid dimension.
        /// </summary>
        /// <remarks>
        /// Each dimension can be considered as valid as soon as 
        /// both values are greater than zero.
        /// </remarks>
        /// <value>
        /// True if this instance is considered as valid dimension 
        /// and false otherwise.
        /// </value>
        public Boolean IsValid
        {
            get
            {
                return this.Width > 0 && this.Lines > 0;
            }
        }

        /// <summary>
        /// Gets and sets the width of this dimension.
        /// </summary>
        /// <remarks>
        /// The width of a console window buffer is calculated 
        /// in number of characters.
        /// </remarks>
        /// <value>
        /// The number of possible characters in X-direction.
        /// </value>
        public Int32 Width { get; set; }

        /// <summary>
        /// Gets and sets the number of lines of this dimension.
        /// </summary>
        /// <remarks>
        /// The number of lines of a console window buffer actually 
        /// represent the maximum number of text lines that can be 
        /// printed out in a console window.
        /// </remarks>
        /// <value>
        /// The number of possible lines in Y-direction.
        /// </value>
        public Int32 Lines { get; set; }

        #endregion

        #region Public methods

        /// <inheritdoc />
        /// <remarks>
        /// This method has been overwritten by this class.
        /// </remarks>
        /// <param name="other">
        /// The object to be compared.
        /// </param>
        public override Boolean Equals(Object other)
        {
            if (Object.ReferenceEquals(null, other))
            {
                return false;
            }

            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }

            return this.GetType() == other.GetType() && this.GetHashCode() == other.GetHashCode();
        }

        /// <inheritdoc />
        /// <remarks>
        /// This method has been overwritten by this class.
        /// </remarks>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                return (this.Lines.GetHashCode() * 521) ^ this.Width.GetHashCode();
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// This method has been overwritten by this class.
        /// </remarks>
        public override String ToString()
        {
            return $"{nameof(this.IsValid)}={this.IsValid}, {nameof(this.Width)}={this.Width}, {nameof(this.Lines)}={this.Lines}";
        }

        #endregion

        #region Operator section

        /// <summary>
        /// Compares two instances of type <see cref="Dimension"/> for equality.
        /// </summary>
        /// <remarks>
        /// Both instances are considered as equal either if both are null or 
        /// if the hash code of both instances is equal.
        /// </remarks>
        /// <param name="x">
        /// The left instance to be compared.
        /// </param>
        /// <param name="y">
        /// The right instance to be compared.
        /// </param>
        /// <returns>
        /// True if both instance are equal, and false otherwise.
        /// </returns>
        /// <seealso cref="Dimension.Equals(Object)"/>
        public static Boolean operator ==(Dimension x, Dimension y)
        {
            if (Object.ReferenceEquals(x, null))
            {
                return Object.ReferenceEquals(y, null);
            }

            return x.Equals(y);
        }

        /// <summary>
        /// Compares two instances of type <see cref="Dimension"/> for inequality.
        /// </summary>
        /// <remarks>
        /// In fact, this is nothing else but the opposite of the equals operator.
        /// </remarks>
        /// <param name="x">
        /// The left instance to be compared.
        /// </param>
        /// <param name="y">
        /// The right instance to be compared.
        /// </param>
        /// <returns>
        /// True if both instance are not equal, and false otherwise.
        /// </returns>
        public static Boolean operator !=(Dimension x, Dimension y)
        {
            return !(x == y);
        }

        #endregion
    }
}
