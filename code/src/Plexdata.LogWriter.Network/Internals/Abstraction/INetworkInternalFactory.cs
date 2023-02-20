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

using Plexdata.LogWriter.Abstraction;
using System;

namespace Plexdata.LogWriter.Internals.Abstraction
{
    /// <summary>
    /// An interface to create internal dependencies.
    /// </summary>
    /// <remarks>
    /// This interface represents the abstraction of a 
    /// factory to create internal dependencies.
    /// </remarks>
    internal interface INetworkInternalFactory
    {
        /// <summary>
        /// Creates instances of internal interface types.
        /// </summary>
        /// <remarks>
        /// This method tries to create instances of internal interface types.
        /// </remarks>
        /// <typeparam name="TInterface">
        /// The interface type to be created.
        /// </typeparam>
        /// <param name="options">
        /// An optional array with additional options. The content and order 
        /// depends on type of <typeparamref name="TInterface"/>
        /// </param>
        /// <returns>
        /// An instance of an internal interface type.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// This exception is thrown if <typeparamref name="TInterface"/> is not 
        /// one of the currently supported interface types that can be created by 
        /// this factory.
        /// </exception>
        TInterface Create<TInterface>(params Object[] options);

        /// <summary>
        /// Creates instances of internal interface types.
        /// </summary>
        /// <remarks>
        /// This method tries to create instances of internal interface types.
        /// </remarks>
        /// <typeparam name="TInterface">
        /// The interface type to be created.
        /// </typeparam>
        /// <param name="settings">
        /// The settings to be used.
        /// </param>
        /// <returns>
        /// An instance of an internal interface type.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// This exception is thrown if <typeparamref name="TInterface"/> is not 
        /// one of the currently supported interface types that can be created by 
        /// this factory.
        /// </exception>
        TInterface Create<TInterface>(INetworkLoggerSettings settings);
    }
}
