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

using Plexdata.LogWriter.Internals.Abstraction;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Threading;

namespace Plexdata.LogWriter.Internals.Facades
{
    /// <summary>
    /// A class to allow physical data transmission.
    /// </summary>
    /// <remarks>
    /// This class represents the implementation of a 
    /// facade of a WEB/HTTP client.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    internal class ClientFacade : IClientFacade
    {
        #region Private fields

        /// <summary>
        /// The target URL.
        /// </summary>
        /// <remarks>
        /// This field holds the used target URL.
        /// </remarks>
        private readonly Uri target;

        /// <summary>
        /// The request method.
        /// </summary>
        /// <remarks>
        /// This field holds the used request method. The value 
        /// may one of the valid HTTP verbs, such as POST.
        /// </remarks>
        private readonly String method;

        /// <summary>
        /// The request content type.
        /// </summary>
        /// <remarks>
        /// This field holds the used request content type.
        /// </remarks>
        private readonly String content;

        /// <summary>
        /// The request timeout.
        /// </summary>
        /// <remarks>
        /// This field holds the used request timeout
        /// </remarks>
        private readonly Int32 timeout;

        #endregion

        #region Construction

        /// <summary>
        /// The class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor initializes its dependencies.
        /// </remarks>
        /// <param name="target">
        /// The target URL to use.
        /// </param>
        /// <param name="method">
        /// The request method to use.
        /// </param>
        /// <param name="content">
        /// The request content type to use.
        /// </param>
        /// <param name="timeout">
        /// The request timeout to use.
        /// </param>
        public ClientFacade(Uri target, String method, String content, Int32 timeout)
            : base()
        {
            this.target = target;
            this.method = method;
            this.content = content;
            this.timeout = timeout;
        }

        #endregion

        #region Public methods

        /// <inheritdoc />
        public Int32 Send(Byte[] payload)
        {
            // Note: Class WebRequest has been marked as obsolete since .NET 6.

            // Unfortunately, it is required to create an instance of
            // HttpWebRequest for each single call. Otherwise, setting
            // the content length twice causes an exception.
            HttpWebRequest request = WebRequest.CreateHttp(this.target);

            request.Method = this.method;
            request.Timeout = this.timeout;
            request.ContentType = this.content;
            request.ContentLength = payload.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(payload, 0, payload.Length);
                stream.Flush();
                stream.Close();
            }

            // Reading back the response is also necessary.
            // Otherwise, a memory leak would be the result.
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (this.IsSuccessful(response))
                {
                    return payload.Length;
                }
            }

            return 0;
        }

        /// <summary>
        /// Frees all disposable instance resources.
        /// </summary>
        /// <remarks>
        /// This method frees all disposable instance resources.
        /// </remarks>
        public void Dispose()
        {
            // There is nothing to do here.
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Checks if data transmission was successful.
        /// </summary>
        /// <remarks>
        /// This method checks if data transmission was successful 
        /// or not by determining the response status code.
        /// </remarks>
        /// <param name="response">
        /// The response to be checked.
        /// </param>
        /// <returns>
        /// True if response is not null and its status code is in 
        /// range of [200...299] and false otherwise.
        /// </returns>
        private Boolean IsSuccessful(HttpWebResponse response)
        {
            if (response == null) { return false; }

            Int32 status = (Int32)response.StatusCode;

            return status >= 200 && status <= 299;
        }

        #endregion
    }
}
