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

namespace Plexdata.Utilities.Assembly
{
    public class AssemblyInfo
    {
        public AssemblyInfo()
        : base()
        {
        }

        public AssemblyInfo(System.Reflection.Assembly assembly)
            : this()
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            this.Title = assembly.GetTitle();
            this.Company = assembly.GetCompany();
            this.Copyright = assembly.GetCopyright();
            this.Product = assembly.GetProduct();
            this.FileName = assembly.GetFileName();
            this.Version = assembly.GetVersion();
            this.Description = assembly.GetDescription();
        }

        public String Title { get; set; }

        public String Company { get; set; }

        public String Copyright { get; set; }

        public String Product { get; set; }

        public String FileName { get; set; }

        public String Version { get; set; }

        public String Description { get; set; }

        public override String ToString()
        {
            const String separator = ", ";

            StringBuilder builder = new StringBuilder(128);

            if (!String.IsNullOrWhiteSpace(this.Title))
            {
                builder.Append($"{this.Title}{separator}");
            }

            if (!String.IsNullOrWhiteSpace(this.Company))
            {
                builder.Append($"{this.Company}{separator}");
            }

            if (!String.IsNullOrWhiteSpace(this.Copyright))
            {
                builder.Append($"{this.Copyright}{separator}");
            }

            if (!String.IsNullOrWhiteSpace(this.Product))
            {
                builder.Append($"{this.Product}{separator}");
            }

            if (!String.IsNullOrWhiteSpace(this.FileName))
            {
                builder.Append($"{this.FileName}{separator}");
            }

            if (!String.IsNullOrWhiteSpace(this.Version))
            {
                builder.Append($"{this.Version}{separator}");
            }

            if (!String.IsNullOrWhiteSpace(this.Description))
            {
                builder.Append($"{this.Description}{separator}");
            }

            return builder.ToString().TrimEnd(separator.ToCharArray());
        }
    }
}
