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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Plexdata.LogWriter.Testing.Helper.Logging
{
    public class LoggerDataDetailConverter : JsonConverter
    {
        public LoggerDataDetailConverter()
            : base()
        {
        }

        public override Boolean CanConvert(Type type)
        {
            return type == typeof(LoggerDataDetail);
        }

        public override Boolean CanRead { get { return true; } }

        public override Boolean CanWrite { get { return false; } }

        public override Object ReadJson(JsonReader reader, Type type, Object existing, JsonSerializer serializer)
        {
            if (!this.CanConvert(type))
            {
                throw new NotSupportedException();
            }

            JObject data = JObject.Load(reader);

            if (data != null && data.HasValues)
            {
                if (data.First is JProperty item && item.HasValues)
                {
                    return new LoggerDataDetail(item.Name, item.First.ToObject(typeof(Object)));
                }
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
