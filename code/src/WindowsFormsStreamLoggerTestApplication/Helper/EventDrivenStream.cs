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
using System.IO;
using System.Text;

namespace Plexdata.LogWriter.Testing.Helper.Helper
{
    public class EventDrivenStream : Stream
    {
        public event StreamEventHandler StreamDataWritten;

        public EventDrivenStream()
            : this(Encoding.UTF8)
        {
        }

        public EventDrivenStream(Encoding encoding)
            : base()
        {
            this.Encoding = encoding;
        }

        public Encoding Encoding { get; set; }

        public override Boolean CanRead
        {
            get
            {
                return false;
            }
        }

        public override Boolean CanSeek
        {
            get
            {
                return false;
            }
        }

        public override Boolean CanWrite
        {
            get
            {
                return true;
            }
        }

        public override Int64 Length
        {
            get
            {
                return 0;
            }
        }

        public override Int64 Position
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        public override void Flush()
        {
        }

        public override Int32 Read(Byte[] buffer, Int32 offset, Int32 count)
        {
            return 0;
        }

        public override Int64 Seek(Int64 offset, SeekOrigin origin)
        {
            return 0;
        }

        public override void SetLength(Int64 value)
        {
        }

        public override void Write(Byte[] buffer, Int32 offset, Int32 count)
        {
            if (buffer != null && buffer.Length > 0)
            {
                try
                {
                    String[] messages = this.Encoding
                        .GetString(buffer, offset, count)
                        .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                    this.StreamDataWritten?.Invoke(this, new StreamEventArgs(messages));
                }
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine(exception);
                }
            }
        }
    }
}
