using Liberator.RESTore.Models;
using System;
using System.Collections.Generic;
using System.Net;

namespace Liberator.RESTore
{
    public interface IResultContext
    {
        /// <summary>
        /// The list of assertions.
        /// </summary>
        Dictionary<string, bool> Assertions { get; set; }

        /// <summary>
        /// The returned content
        /// </summary>
        string Content { get; set; }

        /// <summary>
        /// The elapsed execution time.
        /// </summary>
        TimeSpan ElapsedExecutionTime { get; set; }

        /// <summary>
        /// The headers returned by the response
        /// </summary>
        Dictionary<string, IEnumerable<string>> Headers { get; set; }

        /// <summary>
        /// Whether the response contains a success status
        /// </summary>
        bool IsSuccessStatus { get; set; }

        /// <summary>
        /// The responses from a load test.
        /// </summary>
        List<LoadResponse> LoadResponses { get; set; }

        /// <summary>
        /// Values returned from Load Testing
        /// </summary>
        Dictionary<string, double> LoadValues { get; set; }

        /// <summary>
        /// The returned status code.
        /// </summary>
        HttpStatusCode StatusCode { get; set; }


        /// <summary>
        /// Asserts whether the body meets the requirements of a lambda function.
        /// </summary>
        /// <param name="assert">The assertion in the form of a lambda function.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        ThenContext AssertBody(Func<string, bool> assert);

        /// <summary>
        /// Assert whether the body meets the requirements of a lambda function.
        /// </summary>
        /// <typeparam name="TContent">The type of object to use to deserialise the body.</typeparam>
        /// <param name="assert">A lambda function representing the test.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        ThenContext AssertBody<TContent>(Func<TContent, bool> assert);


        /// <summary>
        /// Asserts whether the response contains a non-success status.
        /// </summary>
        /// <returns>The ThenContext representing the response message.</returns>
        ThenContext AssertFailureStatus();

        /// <summary>
        /// Assert whether the header values meets the requirements of a lambda function.
        /// </summary>
        /// <param name="headerType">The header that will be assessed.</param>
        /// <param name="assert">The assertion in the form of a lambda function.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        ThenContext AssertHeader(string headerType, Func<IEnumerable<string>, bool> assert);

        /// <summary>
        /// Assesses if a header has a specified value.
        /// </summary>
        /// <param name="headerType">The header type to look for.</param>
        /// <param name="value">The value of the header to assert.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        ThenContext AssertHeader(string headerType, string value);

        /// <summary>
        /// Assesses a series of headers and asserts specific values.
        /// </summary>
        /// <param name="headers">A collection of headers and their target values.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        ThenContext AssertHeaders(Dictionary<string, string> headers);

        /// <summary>
        /// Assesses whether the API test passes its validation.
        /// </summary>
        /// <returns>The ThenContext representing the response message.</returns>
        ThenContext AssertPass();

        /// <summary>
        /// Assess whether the status meets the requirements of a lambda function.
        /// </summary>
        /// <param name="assert">The assertion in the form of a lambda function.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        ThenContext AssertStatus(Func<int, bool> assert);

        /// <summary>
        /// Asserts if the Status Code in the response is as anticipated.
        /// </summary>
        /// <param name="httpStatusCode">The code anticipated by the test.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        ThenContext AssertStatus(HttpStatusCode httpStatusCode);

        /// <summary>
        /// Asserts if the Status Code in the response is as anticipated.
        /// </summary>
        /// <param name="statusCode">The code anticipated by the test.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        ThenContext AssertStatus(int statusCode);

        /// <summary>
        /// Asserts whether the response contains a successful status.
        /// </summary>
        /// <returns>The ThenContext representing the response message.</returns>
        ThenContext AssertSuccessStatus();




        /// <summary>
        /// Assess whether the body meets the requirements of a lambda function.
        /// </summary>
        /// <param name="testName">The name of the test.</param>
        /// <param name="assert">The assertion in the form of a lambda function.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        ThenContext AssessBody(string testName, Func<string, bool> assert);

        /// <summary>
        /// Assess whether the body meets the requirements of a lambda function.
        /// </summary>
        /// <typeparam name="TContent">The type of object to use to deserialise the body.</typeparam>
        /// <param name="testName">The name of the test.</param>
        /// <param name="assert">A lambda function representing the test.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        ThenContext AssessBody<TContent>(string testName, Func<TContent, bool> assert);

        /// <summary>
        /// Assess whether the response contains a non-success status.
        /// </summary>
        /// <returns>The ThenContext representing the response message.</returns>
        ThenContext AssessFailureStatus();

        /// <summary>
        /// Assesses if a header has a specified value.
        /// </summary>
        /// <param name="headerType">The header type to look for.</param>
        /// <param name="value">The value of the header to assert.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        ThenContext AssessHeader(string headerType, string value);

        /// <summary>
        /// Assess whether the header values meets the requirements of a lambda function.
        /// </summary>
        /// <param name="testName">The name of the test.</param>
        /// <param name="headerType">The header that will be assessed.</param>
        /// <param name="assert">The assertion in the form of a lambda function.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        ThenContext AssessHeader(string testName, string headerType, Func<IEnumerable<string>, bool> assert);

        /// <summary>
        /// Assesses a series of headers and asserts specific values.
        /// </summary>
        /// <param name="headers">A collection of headers and their target values.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        ThenContext AssessHeaders(Dictionary<string, string> headers);

        /// <summary>
        /// Assesses if the Status Code in the response is as anticipated.
        /// </summary>
        /// <param name="httpStatusCode">The code anticipated by the test.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        ThenContext AssessStatus(HttpStatusCode httpStatusCode);

        /// <summary>
        /// Assesses if the Status Code in the response is as anticipated.
        /// </summary>
        /// <param name="statusCode">The code anticipated by the test.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        ThenContext AssessStatus(int statusCode);

        /// <summary>
        /// Assess whether the status meets the requirements of a lambda function.
        /// </summary>
        /// <param name="testName">The name of the test.</param>
        /// <param name="assert">The assertion in the form of a lambda function.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        ThenContext AssessStatus(string testName, Func<int, bool> assert);

        /// <summary>
        /// Asserts whether the response contains a successful status.
        /// </summary>
        /// <returns>The ThenContext representing the response message.</returns>
        ThenContext AssessSuccessStatus();

        /// <summary>
        /// Get the content in the response body.
        /// </summary>
        /// <param name="content">The resulting content to output.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        ThenContext GetContent(out string content);

        /// <summary>
        /// Gets the response body as an object of the specified type.
        /// </summary>
        /// <typeparam name="TContent">The type of the content being deserialized.</typeparam>
        /// <param name="result">The resulting object to output.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        ThenContext GetJsonObject<TContent>(out TContent result);

        /// <summary>
        /// Gets the response body as an object of the specified type.
        /// </summary>
        /// <typeparam name="TContent">The type of the content being deserialized.</typeparam>
        /// <param name="result">The resulting object to output.</param>
        /// <returns>The ThenContext representing the response message.</returns>
        ThenContext GetXmlObject<TContent>(out TContent result);
    }
}