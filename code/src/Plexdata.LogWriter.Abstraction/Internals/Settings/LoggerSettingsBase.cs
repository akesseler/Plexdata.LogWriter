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
using System.Xml;
using System.Xml.Linq;

namespace Plexdata.LogWriter.Internals.Settings
{
    /// <summary>
    /// The base class of all supported logger settings sections.
    /// </summary>
    /// <remarks>
    /// This internal abstract class serves as the base class of all supported logger settings 
    /// sections. Furthermore, this class does the actual work of section, subsection and value 
    /// management.
    /// </remarks>
    internal abstract class LoggerSettingsBase
    {
        #region Private fields

        /// <summary>
        /// This field contains the list of supported section separators.
        /// </summary>
        /// <remarks>
        /// The list of supported section separators just contains a colon at the moment.
        /// </remarks>
        private readonly Char[] separators = new Char[] { ':' };

        #endregion

        #region Construction

        /// <summary>
        /// The default class constructor.
        /// </summary>
        /// <remarks>
        /// This default constructor just initializes all properties with its default 
        /// values.
        /// </remarks>
        protected LoggerSettingsBase()
            : base()
        {
            this.Parent = null;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The path of this configuration section.
        /// </summary>
        /// <remarks>
        /// The section path represents the hierarchical position inside the underlying 
        /// configuration.
        /// </remarks>
        /// <value>
        /// The section path or an empty string.
        /// </value>
        public String Path
        {
            get
            {
                String result = String.Empty;
                XElement current = this.Parent;

                while (current != null && current.Parent != null)
                {
                    result = $"{current.Name.LocalName}{this.separators[0]}{result}";
                    current = current.Parent;
                }

                return result.TrimEnd(this.separators[0]);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <remarks>
        /// This method builds and returns a string containing current path as well as 
        /// the number of child elements.
        /// </remarks>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override String ToString()
        {
            String path = this.Path;

            if (String.IsNullOrWhiteSpace(path))
            {
                path = "<empty>";
            }

            Int32 children = 0;

            if (this.Parent != null)
            {
                children = this.Parent.Elements().Count();
            }

            return $"Path: {path}, Children: {children}";
        }

        #endregion

        #region Protected properties

        /// <summary>
        /// Gets and sets the parent configuration section.
        /// </summary>
        /// <remarks>
        /// This property gets and sets the parent configuration section. The parent configuration 
        /// section is actually used to manage all values and other subsections.
        /// </remarks>
        /// <value>
        /// The parent configuration section or <c>null</c>.
        /// </value>
        protected XElement Parent { get; set; }

        #endregion

        #region Protected methods

        /// <summary>
        /// Loads the configuration from provided stream.
        /// </summary>
        /// <remarks>
        /// Task of this method is to load the configuration from provided stream. 
        /// This method must be implemented by derived classes. The method should 
        /// not throw any exception.
        /// </remarks>
        /// <param name="stream">
        /// The stream to load the configuration from.
        /// </param>
        /// <returns>
        /// The root element of the loaded configuration or <c>null</c> in case of 
        /// an error.
        /// </returns>
        /// <seealso cref="Load(XmlReader)"/>
        protected abstract XElement Load(Stream stream);

        /// <summary>
        /// Loads the configuration from provided reader.
        /// </summary>
        /// <remarks>
        /// This method loads the configuration root element from provided reader.
        /// The method may return <c>null</c> in any case of an error but does never 
        /// throw any exception.
        /// </remarks>
        /// <param name="reader">
        /// The reader to load the configuration from.
        /// </param>
        /// <returns>
        /// The root element of the loaded configuration or <c>null</c> in case of 
        /// an error.
        /// </returns>
        /// <seealso cref="Load(Stream)"/>
        protected XElement Load(XmlReader reader)
        {
            if (reader == null)
            {
                return null;
            }

            try
            {
                return XDocument.Load(reader).Root;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Finds a configuration subsection assigned to provided key.
        /// </summary>
        /// <remarks>
        /// This method tries to find a configuration subsection assigned to provided key.
        /// The <paramref name="key"/> may contain multiple section parts but each of them 
        /// separated by one of the supported section <see cref="separators"/>.
        /// </remarks>
        /// <param name="key">
        /// The key to find a section for.
        /// </param>
        /// <returns>
        /// The found configuration section or <c>null</c> in case of an error.
        /// </returns>
        /// <seealso cref="FindSection(String[])"/>
        protected XElement FindSection(String key)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            return this.FindSection(key.Split(this.separators, StringSplitOptions.RemoveEmptyEntries));
        }

        /// <summary>
        /// Finds a configuration subsection assigned to provided key hierarchy.
        /// </summary>
        /// <remarks>
        /// This method tries to find a configuration subsection assigned to provided key 
        /// hierarchy.
        /// </remarks>
        /// <param name="keys">
        /// The key hierarchy to find a section for.
        /// </param>
        /// <returns>
        /// The found configuration section or <c>null</c> in case of an error.
        /// </returns>
        /// <seealso cref="FindSection(String)"/>
        protected XElement FindSection(String[] keys)
        {
            if (this.Parent == null || keys == null || keys.Length == 0)
            {
                return null;
            }

            return this.FindSection(new Queue<String>(keys), this.Parent.Elements());
        }

        /// <summary>
        /// Finds a configuration value assigned to section of provided key.
        /// </summary>
        /// <remarks>
        /// This method tries to find a configuration value assigned to section of provided 
        /// key. The returned value might be an empty string.
        /// </remarks>
        /// <param name="key">
        /// The key to get a value for.
        /// </param>
        /// <returns>
        /// The value assigned to provided key or an empty string in any case of an error.
        /// </returns>
        protected String FindValue(String key)
        {
            if (this.Parent == null || String.IsNullOrWhiteSpace(key))
            {
                return String.Empty;
            }

            foreach (XElement element in this.Parent.Elements())
            {
                if (this.IsEqual(element.Name.LocalName, key))
                {
                    XNode node = element.FirstNode;

                    if (node != null && node.NodeType == XmlNodeType.Text)
                    {
                        return (node as XText).Value;
                    }

                    return String.Empty;
                }
            }

            return String.Empty;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Finds a configuration subsection assigned to provided key hierarchy.
        /// </summary>
        /// <remarks>
        /// This method is called recursively until all <paramref name="keys"/> has been 
        /// processed or the list of <paramref name="items"/> does not contain any child.
        /// </remarks>
        /// <param name="keys">
        /// The FIFO of key hierarchies to be processed.
        /// </param>
        /// <param name="items">
        /// The list of child elements of previous parent element.
        /// </param>
        /// <returns>
        /// The found configuration section or <c>null</c> in case of an error.
        /// </returns>
        /// <seealso cref="FindSection(String)"/>
        /// <seealso cref="FindSection(String[])"/>
        private XElement FindSection(Queue<String> keys, IEnumerable<XElement> items)
        {
            if (keys.Count > 0)
            {
                String name = keys.Dequeue();

                foreach (XElement item in items)
                {
                    if (this.IsEqual(item.Name.LocalName, name))
                    {
                        if (keys.Count > 0)
                        {
                            return this.FindSection(keys, item.Elements());
                        }
                        else
                        {
                            return item;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Determines the equality of two string.
        /// </summary>
        /// <remarks>
        /// This method determines the equality of two string by comparing them without 
        /// case sensitivity.
        /// </remarks>
        /// <param name="x">
        /// The left string to be compared.
        /// </param>
        /// <param name="y">
        /// The right string to be compared.
        /// </param>
        /// <returns>
        /// True, if both strings are considered as equal and false otherwise.
        /// </returns>
        private Boolean IsEqual(String x, String y)
        {
            return String.Compare(x, y, StringComparison.OrdinalIgnoreCase) == 0;
        }

        #endregion
    }
}
