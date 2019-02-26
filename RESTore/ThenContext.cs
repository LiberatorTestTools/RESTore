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


using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Liberator.RESTore
{
    /// <summary>
    /// The response for the request.
    /// </summary>
    public class ThenContext
    {
        #region Properties

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
        /// Whether the response contains a success status
        /// </summary>
        public bool IsSuccessStatus { get; set; } 

        #endregion

        #region Constructors

        /// <summary>
        /// The Constructor for the context
        /// </summary>
        public ThenContext()
        {
            Headers = new Dictionary<string, IEnumerable<string>>();
            LoadValues = new Dictionary<string, double>();
            Assertions = new Dictionary<string, bool>();
        } 

        #endregion

        #region Assertions

        /// <summary>
        /// Assesses if a header has a specified value.
        /// </summary>
        /// <param name="headerType">The header type to look for.</param>
        /// <param name="value">The value of the header to assert.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        public ThenContext AssessHeader(string headerType, string value)
        {
            CheckIfHeaderValueIsConfirmed(headerType, value);
            return this;
        }


        /// <summary>
        /// Checks if a header has a specified value.
        /// </summary>
        /// <param name="headerType">The header type to look for.</param>
        /// <param name="value">The value of the header to assert.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        public ThenContext AssertHeader(string headerType, string value)
        {
            RESToreSettings.Log.WriteLine("BEGIN AssertHeader test");

            bool isHeaderPresent = Headers.ContainsKey(headerType);
            RESToreSettings.Log.WriteLine($"{headerType} is present in the response: {isHeaderPresent}");

            bool doesHeaderHaveValue = isHeaderPresent && Headers[headerType].Contains(value);
            RESToreSettings.Log.WriteLine($"{headerType} contains {value}: {doesHeaderHaveValue}");
            
            Assert.That(doesHeaderHaveValue, Is.True, $"{headerType} does not contain value {value}");

            RESToreSettings.Log.WriteLine("AssertHeader test finished successfully");
            RESToreSettings.Log.WriteLine();
            return this;
        }


        /// <summary>
        /// Assesses a series of headers and asserts specific values.
        /// </summary>
        /// <param name="headers">A collection of headers and their target values.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        public ThenContext AssessHeaders(Dictionary<string, string> headers)
        {
            foreach (KeyValuePair<string, string> header in headers)
            {
                CheckIfHeaderValueIsConfirmed(header.Key, header.Value);
            }
            return this;
        }


        /// <summary>
        /// Checks a series of headers and asserts specific values.
        /// </summary>
        /// <param name="headers">A collection of headers and their target values.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        public ThenContext AssertHeaders(Dictionary<string, string> headers)
        {
            RESToreSettings.Log.WriteLine("BEGIN AssertHeaders test");

            foreach (KeyValuePair<string, string> header in headers)
            {
                RESToreSettings.Log.WriteLine();

                bool isHeaderPresent = Headers.ContainsKey(header.Key);
                RESToreSettings.Log.WriteLine($"{header.Key} is present in the response: {isHeaderPresent}");

                bool doesHeaderHaveValue = isHeaderPresent && Headers[header.Key].Contains(header.Value);
                RESToreSettings.Log.WriteLine($"{header.Key} contains {header.Value}: {doesHeaderHaveValue}");

                Assert.That(doesHeaderHaveValue, Is.True, $"{header.Key} does not contain value {header.Value}");
            }

            RESToreSettings.Log.WriteLine("AssertHeaders test finished successfully");
            RESToreSettings.Log.WriteLine();
            return this;
        }


        /// <summary>
        /// Assesses if the Status Code in the response is as anticipated.
        /// </summary>
        /// <param name="httpStatusCode">The code anticipated by the test.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        public ThenContext AssessStatus(HttpStatusCode httpStatusCode)
        {
            AddAndLogAssessment($"Expected HTTP Status: {httpStatusCode} to be equal to actual status code: {StatusCode}", StatusCode.Equals(httpStatusCode));
            return this;
        }


        /// <summary>
        /// Asserts if the Status Code in the response is as anticipated.
        /// </summary>
        /// <param name="httpStatusCode">The code anticipated by the test.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        public ThenContext AssertStatus(HttpStatusCode httpStatusCode)
        {
            RESToreSettings.Log.WriteLine("BEGIN AssertStatus test");
            Assert.That(StatusCode.Equals(httpStatusCode), Is.True, $"Expected HTTP Status: {httpStatusCode} but actual status is: {StatusCode}");
            RESToreSettings.Log.WriteLine("AssertStatus test finished successfully");
            RESToreSettings.Log.WriteLine();
            return this;
        }


        /// <summary>
        /// Asserts whether the response contains a successful status
        /// </summary>
        /// <returns></returns>
        public ThenContext AssessSuccessStatus()
        {
            AddAndLogAssessment("Responded with success status", IsSuccessStatus);
            return this;
        }


        /// <summary>
        /// Asserts whether the response contains a successful status
        /// </summary>
        /// <returns></returns>
        public ThenContext AssertSuccessStatus()
        {
            RESToreSettings.Log.WriteLine("BEGIN AssertSuccessStatus test");
            Assert.That(IsSuccessStatus, Is.True, $"HTTP Status: {StatusCode} does not indicate success");
            RESToreSettings.Log.WriteLine("AssertSuccessStatus test finished successfully");
            RESToreSettings.Log.WriteLine();
            return this;
        }


        /// <summary>
        /// Assess whether the response contains a non-success status
        /// </summary>
        /// <returns></returns>
        public ThenContext AssessFailureStatus()
        {
            AddAndLogAssessment("Responded with failure status", !IsSuccessStatus);
            return this;
        }


        /// <summary>
        /// Asserts whether the response contains a non-success status
        /// </summary>
        /// <returns></returns>
        public ThenContext AssertFailureStatus()
        {
            RESToreSettings.Log.WriteLine("BEGIN AssertFailureStatus test");
            Assert.That(IsSuccessStatus, Is.False, $"HTTP Status: {StatusCode} does not indicate failure");
            RESToreSettings.Log.WriteLine("AssertFailureStatus test finished successfully");
            RESToreSettings.Log.WriteLine();
            return this;
        }

        /// <summary>
        /// Assess whether the body meets the requirements of a lambda function
        /// </summary>
        /// <param name="testName">The name of the test</param>
        /// <param name="assert">The assertion in the form of a lambda function</param>
        /// <returns>The ThenContext representing the response message.</returns>
        public ThenContext AssessBody(string testName, Func<string, bool> assert)
        {
            RESToreSettings.Log.WriteLine($"START AssessBody test: {testName} with user function");
            bool result;
            try
            {
                RESToreSettings.Log.WriteLine("Calling user assert function...");
                result = assert(Content);
                RESToreSettings.Log.WriteLine("User function returned without exception");
            }
            catch(Exception e)
            {
                RESToreSettings.Log.WriteLine($"User function threw an exception: {e.Message}");
                result = false;
            }

            AddAndLogAssessment(testName, result);
            RESToreSettings.Log.WriteLine("FINISH AssessBody test");
            RESToreSettings.Log.WriteLine();
            return this;
        }

        /// <summary>
        /// Asserts whether the body meets the requirements of a lambda function
        /// </summary>
        /// <param name="assert">The assertion in the form of a lambda function</param>
        /// <returns>The ThenContext representing the response message.</returns>
        public ThenContext AssertBody(Func<string, bool> assert)
        {
            RESToreSettings.Log.WriteLine("BEGIN AssertBody test with user function");
            bool result;
            try
            {
                RESToreSettings.Log.WriteLine("Calling user assert function...");
                result = assert(Content);
                RESToreSettings.Log.WriteLine("User function returned without exception");
            }
            catch(Exception e)
            {
                RESToreSettings.Log.WriteLine($"User function threw an exception: {e.Message}");
                result = false;
            }

            Assert.That(result, Is.True, "Condition used for Assert Body has failed");
            RESToreSettings.Log.WriteLine("AssessBody test finished successfully");
            RESToreSettings.Log.WriteLine();
            return this;
        }

        /// <summary>
        /// Assess whether the body contains a particular type of object
        /// </summary>
        /// <typeparam name="TContent">The type of object to use to deserialise the body.</typeparam>
        /// <param name="testName">The name of the test</param>
        /// <param name="assert">A lambda function representing the test.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        public ThenContext AssessBody<TContent>(string testName, Func<TContent, bool> assert)
        {
            RESToreSettings.Log.WriteLine($"START AssessBody test: {testName} with user function");
            bool result;
            try
            {
                RESToreSettings.Log.WriteLine("Calling user assert function...");
                result = assert(JsonConvert.DeserializeObject<TContent>(Content));
                RESToreSettings.Log.WriteLine("User function returned without exception");
            }
            catch(Exception e)
            {
                RESToreSettings.Log.WriteLine($"User function threw an exception: {e.Message}");
                result = false;
            }
            AddAndLogAssessment(testName, result);
            RESToreSettings.Log.WriteLine("FINISH AssessBody test");
            RESToreSettings.Log.WriteLine();
            return this;
        }

        /// <summary>
        /// Asserts whether the body contains a particular type of object
        /// </summary>
        /// <typeparam name="TContent">The type of object to use to deserialise the body.</typeparam>
        /// <param name="assert">A lambda function representing the test.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        public ThenContext AssertBody<TContent>(Func<TContent, bool> assert)
        {
            RESToreSettings.Log.WriteLine("BEGIN AssertBody test with user function");
            bool result;
            try
            {
                RESToreSettings.Log.WriteLine("Calling user assert function...");
                result = assert(JsonConvert.DeserializeObject<TContent>(Content));
                RESToreSettings.Log.WriteLine("User function returned without exception");
            }
            catch(Exception e)
            {
                RESToreSettings.Log.WriteLine($"User function threw an exception: {e.Message}");
                result = false;
            }

            Assert.That(result, Is.True, "Condition used for Assert Body has failed");
            RESToreSettings.Log.WriteLine("AssessBody test finished successfully");
            RESToreSettings.Log.WriteLine();
            return this;
        }

        /// <summary>
        /// Assesses whether the API test passes its validation
        /// </summary>
        /// <returns>The ThenContext representing the response message.</returns>
        public ThenContext AssertPass()
        {
            Assert.That(Assertions.All(x => x.Value == true), Is.True);
            return this;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Checks whether a header contains a particular value.
        /// </summary>
        /// <param name="headerType">The type of header to test.</param>
        /// <param name="value">The value to assert.</param>
        private void CheckIfHeaderValueIsConfirmed(string headerType, string value)
        {
            bool doesHeaderHaveValue = Headers.ContainsKey(headerType) && Headers[headerType].Contains(value);
            AddAndLogAssessment($"Header: {headerType} has Value: {value}", doesHeaderHaveValue);
        }

        private void AddAndLogAssessment(string assessment, bool value)
        {
            Assertions.Add(assessment, value);
            RESToreSettings.Log.WriteLine($"Assessment: {assessment} | {value.ToString().ToUpper()}");
            RESToreSettings.Log.WriteLine();
        }

        #endregion
    }
}
