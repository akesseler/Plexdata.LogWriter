/*
 * MIT License
 * 
 * Copyright (c) 2023 plexdata.de
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

using Plexdata.LogWriter.Internals.Abstraction;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.LogWriter.Internals.Facades
{
    /// <summary>
    /// A class allowing to generate random numbers.
    /// </summary>
    /// <remarks>
    /// This class represents the implementation of a facade 
    /// for class <see cref="Random"/>.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    internal class RandomFacade : IRandomFacade
    {
        #region Private fields

        /// <summary>
        /// An instance of used random number generator.
        /// </summary>
        /// <remarks>
        /// This field holds the instance of used random number generator.
        /// </remarks>
        private readonly Random random = null;

        #endregion

        #region Construction

        /// <summary>
        /// The standard constructor.
        /// </summary>
        /// <remarks>
        /// This constructor initializes its instance of a random 
        /// number generator using current tick count as seed.
        /// </remarks>
        /// <seealso cref="Random"/>
        /// <seealso cref="Environment.TickCount"/>
        public RandomFacade()
            : base()
        {
            this.random = new Random(Environment.TickCount);
        }

        #endregion

        #region Public methods

        /// <inheritdoc />
        public void NextBytes(Byte[] buffer)
        {
            this.random.NextBytes(buffer);
        }

        #endregion
    }
}
