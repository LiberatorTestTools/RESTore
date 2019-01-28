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
using System.Collections.Generic;
using System.Net;

namespace RESTore
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
        /// The list of schema errors
        /// </summary>
        public List<string> SchemaErrors { get; set; }

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
        
    }
}
