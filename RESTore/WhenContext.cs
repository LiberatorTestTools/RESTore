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


using RESTore.Enumerations;
using System;
using System.Runtime.InteropServices;

namespace RESTore
{
    /// <summary>
    /// Represents the HTTP Action being undertaken.
    /// </summary>
    public class WhenContext
    {
        /// <summary>
        /// Whether the test us a load test.
        /// </summary>
        public bool IsLoadTest { get; set; }

        /// <summary>
        /// Which HTTP Verb should be used.
        /// </summary>
        public HTTPVerb HttpVerbUsed { get; set; }

        /// <summary>
        /// The GivenContext representing the setup of the request.
        /// </summary>
        public GivenContext GivenContext { get; set; }

        /// <summary>
        /// The target URL for the request.
        /// </summary>
        public string TargetUrl { get; set; }

        /// <summary>
        /// The number of threads to run a load test.
        /// </summary>
        public int Threads { get; set; }

        /// <summary>
        /// The number of seconds to run a load test.
        /// </summary>
        public int Seconds { get; set; }

        /// <summary>
        /// Constructor for the WehnContext class.
        /// </summary>
        /// <param name="givenContext">The GivenContext object required.</param>
        public WhenContext(GivenContext givenContext)
        {
            GivenContext = givenContext;
        }

        /// <summary>
        /// Allows a user to define the basic parameters of a load test.
        /// </summary>
        /// <param name="seconds">Number of seconds to run the load test for.</param>
        /// <param name="threads">The number of threads to allow during the test.</param>
        /// <returns>The WhenContext object with the load test parameters set.</returns>
        public WhenContext LoadTest([Optional, DefaultParameterValue(60)]int seconds, [Optional, DefaultParameterValue(1)]int threads)
        {
            IsLoadTest = true;
            Threads = threads < 0 ? 1 : threads;
            Seconds = seconds < 0 ? 60 : seconds;
            return this;
        }

        /// <summary>
        /// Allows the user to set and execute a GET request.
        /// </summary>
        /// <param name="url">The URL to send the GET request to.</param>
        /// <returns>The ExecutionContext that represents the executing query.</returns>
        public ExecutionContext Get([Optional, DefaultParameterValue(null)]string url)
        {
            return SetHttpAction(url, HTTPVerb.GET);
        }

        /// <summary>
        /// Allows the user to set and execute a POST request.
        /// </summary>
        /// <param name="url">The URL to send the POST request to.</param>
        /// <returns>The ExecutionContext that represents the executing query.</returns>
        public ExecutionContext Post([Optional, DefaultParameterValue(null)]string url)
        {
            return SetHttpAction(url, HTTPVerb.POST);
        }

        /// <summary>
        /// Allows the user to set and execute a PUT request.
        /// </summary>
        /// <param name="url">The URL to send the PUT request to.</param>
        /// <returns>The ExecutionContext that represents the executing query.</returns>
        public ExecutionContext Put([Optional, DefaultParameterValue(null)]string url)
        {
            return SetHttpAction(url, HTTPVerb.PUT);
        }

        /// <summary>
        /// Allows the user to set and execute a PATCH request.
        /// </summary>
        /// <param name="url">The URL to send the PATCH request to.</param>
        /// <returns>The ExecutionContext that represents the executing query.</returns>
        public ExecutionContext Patch([Optional, DefaultParameterValue(null)]string url)
        {
            return SetHttpAction(url, HTTPVerb.PATCH);
        }

        /// <summary>
        /// Allows the user to set and execute a DELETE request.
        /// </summary>
        /// <param name="url">The URL to send the DELETE request to.</param>
        /// <returns>The ExecutionContext that represents the executing query.</returns>
        public ExecutionContext Delete([Optional, DefaultParameterValue(null)]string url)
        {
            return SetHttpAction(url, HTTPVerb.DELETE);
        }

        /// <summary>
        /// Sets the target URL.
        /// </summary>
        /// <param name="url">The URL to be set.</param>
        /// <returns>The full target URL.</returns>
        private string SetUrl(string url)
        {
            if (string.IsNullOrEmpty(url) && string.IsNullOrEmpty(GivenContext.HostName))
            {
                throw new ArgumentException("URL must be provided");
            }

            TargetUrl = new Uri(new Uri(GivenContext.HostName), url).AbsoluteUri;
            
            return TargetUrl;
        }

        /// <summary>
        /// Sets the HTTP Action
        /// </summary>
        /// <param name="url">The URL to be targetted.</param>
        /// <param name="httpVerb">The verb describing the HTTP Action</param>
        /// <returns>The ExecutionContext that represents the executing query.</returns>
        private ExecutionContext SetHttpAction(string url, HTTPVerb httpVerb)
        {
            SetUrl(url);
            HttpVerbUsed = httpVerb;
            return new ExecutionContext(GivenContext, this);
        }
    }
}
