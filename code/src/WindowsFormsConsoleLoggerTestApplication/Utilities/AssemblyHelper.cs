/*
 * MIT License
 * 
 * Copyright (c) 2021 plexdata.de
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Plexdata.Utilities.Assembly
{
    public static class AssemblyHelper
    {
        public static AssemblyInfo GetCurrentAssemblyInformation(this AppDomain domain)
        {
            try
            {
                return domain.GetAssemblyInformation(System.Reflection.Assembly.GetExecutingAssembly().FullName, false).First();
            }
            catch { }

            return new AssemblyInfo();
        }

        public static IEnumerable<AssemblyInfo> GetAssemblyInformation(this AppDomain domain)
        {
            return domain.GetAssemblyInformation(System.Reflection.Assembly.GetExecutingAssembly().FullName, true);
        }

        public static IEnumerable<AssemblyInfo> GetAssemblyInformation(this AppDomain domain, String affected)
        {
            return domain.GetAssemblyInformation(affected, false);
        }

        public static IEnumerable<AssemblyInfo> GetAssemblyInformation(this AppDomain domain, String affected, Boolean excluded)
        {
            if (domain == null)
            {
                return Enumerable.Empty<AssemblyInfo>();
            }

            try
            {
                IEnumerable<System.Reflection.Assembly> assemblies = Enumerable.Empty<System.Reflection.Assembly>();

                if (String.IsNullOrWhiteSpace(affected))
                {
                    assemblies = domain.GetAssemblies();
                }
                else if (excluded)
                {
                    assemblies = domain.GetAssemblies().Where(x => !x.FullName.StartsWith(affected));
                }
                else
                {
                    assemblies = domain.GetAssemblies().Where(x => x.FullName.StartsWith(affected));
                }

                List<AssemblyInfo> result = new List<AssemblyInfo>();

                foreach (System.Reflection.Assembly current in assemblies)
                {
                    result.Add(new AssemblyInfo(current));
                }

                return result;
            }
            catch { }

            return Enumerable.Empty<AssemblyInfo>();
        }

        public static String GetTitle(this System.Reflection.Assembly assembly)
        {
            try
            {
                Type type = typeof(AssemblyTitleAttribute);

                if (AssemblyTitleAttribute.IsDefined(assembly, type))
                {
                    AssemblyTitleAttribute attribute = (AssemblyTitleAttribute)AssemblyTitleAttribute.GetCustomAttribute(assembly, type);
                    return attribute.Title;
                }
            }
            catch { }

            return String.Empty;
        }

        public static String GetCompany(this System.Reflection.Assembly assembly)
        {
            try
            {
                Type type = typeof(AssemblyCompanyAttribute);
                if (AssemblyCompanyAttribute.IsDefined(assembly, type))
                {
                    AssemblyCompanyAttribute attribute = (AssemblyCompanyAttribute)AssemblyCompanyAttribute.GetCustomAttribute(assembly, type);
                    return attribute.Company;
                }

            }
            catch { }

            return String.Empty;
        }

        public static String GetCopyright(this System.Reflection.Assembly assembly)
        {
            try
            {
                Type type = typeof(AssemblyCopyrightAttribute);
                if (AssemblyCopyrightAttribute.IsDefined(assembly, type))
                {
                    AssemblyCopyrightAttribute attribute = (AssemblyCopyrightAttribute)AssemblyCopyrightAttribute.GetCustomAttribute(assembly, type);
                    return attribute.Copyright;
                }

            }
            catch { }

            return String.Empty;
        }

        public static String GetProduct(this System.Reflection.Assembly assembly)
        {
            try
            {
                Type type = typeof(AssemblyProductAttribute);
                if (AssemblyProductAttribute.IsDefined(assembly, type))
                {
                    AssemblyProductAttribute attribute = (AssemblyProductAttribute)AssemblyProductAttribute.GetCustomAttribute(assembly, type);
                    return attribute.Product;
                }

            }
            catch { }

            return String.Empty;
        }

        public static String GetFileName(this System.Reflection.Assembly assembly)
        {
            if (assembly == null)
            {
                return String.Empty;
            }

            try
            {
                return Path.GetFileName(assembly.Location);
            }
            catch { }

            return String.Empty;
        }

        public static String GetVersion(this System.Reflection.Assembly assembly)
        {
            try
            {
                return assembly.GetName().Version.ToString();
            }
            catch { }

            return String.Empty;
        }

        public static String GetDescription(this System.Reflection.Assembly assembly)
        {
            try
            {
                Type type = typeof(AssemblyDescriptionAttribute);

                if (AssemblyDescriptionAttribute.IsDefined(assembly, type))
                {
                    AssemblyDescriptionAttribute attribute = (AssemblyDescriptionAttribute)AssemblyDescriptionAttribute.GetCustomAttribute(assembly, type);
                    return attribute.Description;
                }
            }
            catch { }

            return String.Empty;
        }
    }
}
