// This version, Copyright 2019 Liberator Test Tools
// 
// Based on original work of the RestAssured.NET project on GitHub
// https://github.com/lamchakchan/RestAssured.Net
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.


using System;
using System.Collections;
using System.Configuration;

namespace RESTore
{
    /// <summary>
    /// Extension methods
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// Fixes the protocol if the request is to a secure endpoint
        /// </summary>
        /// <param name="source">The source URL.</param>
        /// <param name="useHttps">Whether to use HTTPS</param>
        /// <returns>The fixed protocol</returns>
        internal static string FixProtocol(this string source, bool useHttps)
        {
            var defaultPortocol = useHttps ? "https" : "http";
            if (!source.StartsWith("http://") && !source.StartsWith("https://"))
                return $"{defaultPortocol}://" + source;

            return source;
        }

        /// <summary>
        /// Allows a string value to be quoted.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static string Quote(this string source)
        {
            return "\"" + source + "\"";
        }

        /// <summary>
        /// Returns a URI as a string.
        /// </summary>
        /// <param name="source">The source Uri</param>
        /// <returns>The URL as a string</returns>
        internal static string SchemeAndHost(this Uri source)
        {
            return source.Scheme + "://" + source.Host;
        }

        /// <summary>
        /// Checks if a value is present in a dictionary
        /// </summary>
        /// <param name="dictionary">The dictionary being checked.</param>
        /// <param name="value">The value of the key to locate.</param>
        /// <returns>True if the element is found in the dictionary.</returns>
        internal static bool IsPresentInDictionary(this IEnumerable dictionary, object value)
        {
            object objectFound = ((IDictionary)dictionary)[value];
            return objectFound != null ? true : false;
        }

        /// <summary>
        /// Checks if a value is present in a KeyValueConfigurationCollection.
        /// </summary>
        /// <param name="configurationCollection">The collection of settings.</param>
        /// <param name="value">the setting required.</param>
        /// <returns>Whether the setting is in the collection passed.</returns>
        internal static bool IsPresentInCollection(this KeyValueConfigurationCollection configurationCollection, string value)
        {
            object objectFound = configurationCollection[value].Value;
            return objectFound != null ? true : false;
        }
    }
}
