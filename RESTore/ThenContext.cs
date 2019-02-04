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


using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace Liberator.RESTore
{
    /// <summary>
    /// The response for the request.
    /// </summary>
    public class ThenContext
    {
        /// <summary>
        /// The returned status code.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// The returned content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Thenparsed content of the response.
        /// </summary>
        public dynamic ParsedContent { get; set; }

        /// <summary>
        /// The headers returned by the response
        /// </summary>
        public Dictionary<string, IEnumerable<string>> Headers { get; set; }

        /// <summary>
        /// The elapsed execution time.
        /// </summary>
        /// 
        public TimeSpan ElapsedExecutionTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, double> LoadValues { get; set; }

        /// <summary>
        /// The list of assertions.
        /// </summary>
        public Dictionary<string, bool> Assertions { get; set; }

        /// <summary>
        /// The responses from a load test.
        /// </summary>
        public List<LoadResponse> LoadResponses { get; set; }

        /// <summary>
        /// If the returned schema is valid.
        /// </summary>
        public bool IsSchemaValid { get; set; }

        /// <summary>
        /// Whether the response contains a success status
        /// </summary>
        public bool IsSuccessStatus { get; set; }

        /// <summary>
        /// The list of schema errors
        /// </summary>
        public List<string> SchemaErrors { get; set; }

        /// <summary>
        /// The TYpe of Content being returned
        /// </summary>
        private string ContentType { get; set; }


        /// <summary>
        /// The Constructor for the context
        /// </summary>
        public ThenContext()
        {
            Headers = new Dictionary<string, IEnumerable<string>>();
            LoadValues = new Dictionary<string, double>();
            Assertions = new Dictionary<string, bool>();
            IsSchemaValid = true;
            SchemaErrors = new List<string>();
        }


        /// <summary>
        /// Checks if a header has a specified value.
        /// </summary>
        /// <param name="headerType">The header type to look for.</param>
        /// <param name="value">The value of the header to assert.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        public ThenContext AssertHeader(string headerType, string value)
        {
            CheckIfHeaderValueIsConfirmed(headerType, value);

            return this;
        }


        /// <summary>
        /// Checks a series of headers and asserts specific values.
        /// </summary>
        /// <param name="headers">A collection of headers and their target values.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        public ThenContext AssertHeaders(Dictionary<string, string> headers)
        {
            foreach (KeyValuePair<string, string> header in headers)
            {
                CheckIfHeaderValueIsConfirmed(header.Key, header.Value);
            }
            return this;
        }


        /// <summary>
        /// Asserts if the Status Code in the response is as anticipated.
        /// </summary>
        /// <param name="httpStatusCode">The code anticipated by the test.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        public ThenContext AssertStatus(HttpStatusCode httpStatusCode)
        {
            if (StatusCode.Equals(httpStatusCode))
            {
                Assertions.Add(string.Format("Status {0} confirmed.", StatusCode), true);
            }
            else
            {
                Assertions.Add(string.Format("Status {0} not found. Actual status is {1}.", StatusCode, httpStatusCode), false);
            }
            return this;
        }


        /// <summary>
        /// Asserts whether the response contains a successful status
        /// </summary>
        /// <returns></returns>
        public ThenContext AssertSuccessStatus()
        {
            if (IsSuccessStatus)
            {
                Assertions.Add("Success status returned.", true);
            }
            else
            {
                Assertions.Add("Non-success status found.", false);
            }
            return this;
        }

        public ThenContext AssertBody(string testName, Func<string, bool> assert)
        {
            bool result;
            try
            {
                result = assert(Content);
            }
            catch
            {
                result = false;
            }

            Assertions.Add(testName, result);
            return this;
        }

        public ThenContext AssertBody<TContent>(string testName, Func<TContent, bool> assert)
        {
            bool result;
            try
            {
                result = assert(JsonConvert.DeserializeObject<TContent>(Content));
            }
            catch
            {
                result = false;
            }

            Assertions.Add(testName, result);
            return this;
        }

        public void AssertPass()
        {
            Assert.That(Assertions.All(x => x.Value != false), Is.True);
        }


        public ThenContext RetrieveValue(Func<dynamic, object> func)
        {
            try
            {
                return func.Invoke(ParsedContent).Value;
            }
            catch
            {
                try
                {
                    return func.Invoke(ParsedContent);
                }
                catch
                {
                    Debug.WriteLine(string.Format("Unable to retrieve the value specified by {0}", func.ToString()));
                }
            }
            return null;
        }

        /// <summary>
        /// Outputs the list of Assertions and their results to the Debug console
        /// </summary>
        /// <returns>The ThenContext representing the response message.</returns>
        public ThenContext ToConsole()
        {
            foreach (KeyValuePair<string, bool> assertion in Assertions)
            {
                Console.WriteLine(string.Format("Assertion: {0} | {1}", assertion.Key, assertion.Value.ToString().ToUpper()));
            }
            return this;
        }


        /// <summary>
        /// Checks whether a header contains a particular value.
        /// </summary>
        /// <param name="headerType">The type of header to test.</param>
        /// <param name="value">The value to assert.</param>
        private void CheckIfHeaderValueIsConfirmed(string headerType, string value)
        {
            if (Headers.IsPresentInDictionary(headerType) && Headers[headerType].ToList().Contains(value))
            {
                Assertions.Add(string.Format("Header: {0} not found", headerType), false);
            }
            else
            {
                Assertions.Add(string.Format("Header: {0} | Value: {1}", headerType, value), true);
            }
        }


        /// <summary>
        /// Extracts content types and parses the content
        /// </summary>
        public void GetContent()
        {
            ContentType = Headers["Content-Type"].First();
            ParseResponseContent();
        }


        /// <summary>
        /// Parses the JSON or XML content to an object
        /// </summary>
        private void ParseResponseContent()
        {
            if (!string.IsNullOrEmpty(Content))
            {
                if (ContentType.Contains("json"))
                {
                    try
                    {
                        ParsedContent = JObject.Parse(Content);
                        return;
                    }
                    catch
                    {
                        try
                        {
                            ParsedContent = JArray.Parse(Content);
                            return;
                        }
                        catch
                        {
                            throw new RESToreException("Cannot parse the JSON response to either an object or an array");
                        }
                    }

                }
                else if (ContentType.Contains("xml"))
                {
                    try
                    {
                        ParsedContent = XDocument.Parse(Content);
                        return;
                    }
                    catch
                    {
                        throw new RESToreException("Cannot parse the XML response.");
                    }

                }
                else if (ContentType.Contains("html"))
                {
                    try
                    {
                        HtmlDocument document = new HtmlDocument();
                        document.LoadHtml(Content);
                        ParsedContent = document.DocumentNode;
                    }
                    catch
                    {
                        throw new RESToreException("Cannot parse the HTML response.");
                    }
                    return;
                }
            }

            if (!string.IsNullOrEmpty(Content))
                throw new RESToreException(string.Format("The Content-Type {0} is not supported at present.", ContentType));
        }
    }
}
