using Liberator.RESTore.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Xml;

namespace Liberator.RESTore
{
    public interface ISetupContext
    {
        /// <summary>
        /// Allows a user to set the body of the request.
        /// </summary>
        /// <param name="body">The body to be sent with the request.</param>
        /// <returns>The GivenContext object with the request body set.</returns>
        GivenContext Body(string body);

        /// <summary>
        /// Allows a user to set the body of the request using an XML Document
        /// </summary>
        /// <param name="xmlDocument">Document object to be used in the call.</param>
        /// <returns>The GivenContext object with the request body set.</returns>
        GivenContext Body(XmlDocument xmlDocument);

        /// <summary>
        /// Allows a user to add a body based on an Object and serialises it.
        /// </summary>
        /// <typeparam name="T">The type of object being passed.</typeparam>
        /// <param name="body">The body object.</param>
        /// <returns>The GivenContext object with the request body set.</returns>
        GivenContext Body<T>(T body) where T : class;

        /// <summary>
        /// Allows a user to add an XML body based on an Object and serialises it.
        /// </summary>
        /// <typeparam name="T">The type of object being passed.</typeparam>
        /// <param name="body">The body object to be converted to XML.</param>
        /// <returns>The GivenContext object with the request body set.</returns>
        GivenContext BodyXML<T>(T body) where T : class;

        /// <summary>
        /// Allows a user to add a single cookie to a request.
        /// </summary>
        /// <param name="name">The name of the cookie parameter.</param>
        /// <param name="value">The value of the cookie parameter.</param>
        /// <returns>The GivenContext representing the setup of the request.</returns>
        GivenContext Cookie(string name, string value);

        /// <summary>
        /// Allows a user to add a collection of cookies to a request.
        /// </summary>
        /// <param name="cookies">The dictionary representing the cookies for the request.</param>
        /// <returns>The GivenContext representing the setup of the request.</returns>
        GivenContext Cookies(Dictionary<string, string> cookies);

        /// <summary>
        /// Allows a user to add a file to the request.
        /// </summary>
        /// <param name="fileName">The name of the file to add.</param>
        /// <param name="contentDispositionName">A header that indicates if the file is to displayed inline in the browser.</param>
        /// <param name="contentType">The content-type of the file.</param>
        /// <param name="content">The content of the file as a byte array.</param>
        /// <returns>The GivenContext representing the setup of the request.</returns>
        GivenContext File(string fileName, string contentDispositionName, string contentType, byte[] content);

        /// <summary>
        /// Allows a user to add a single header to a request.
        /// </summary>
        /// <param name="headerType">The type of header to use.</param>
        /// <param name="value">The value to use for the header.</param>
        /// <returns>The GivenContext representing the setup of the request.</returns>
        GivenContext Header(string headerType, string value);

        /// <summary>
        /// Allows a user to add a collection of headers to a request.
        /// </summary>
        /// <param name="headers">The dictionary representing the query parameters for the request.</param>
        /// <returns>The GivenContext representing the setup of the request.</returns>
        GivenContext Headers(Dictionary<string, string> headers);

        /// <summary>
        /// Allows a user to set the name of the host to use for the test suite.
        /// </summary>
        /// <param name="host">The name of the host to use for the test suite.</param>
        /// <returns>The GivenContext object with the host name set.</returns>
        GivenContext Host(string host);

        /// <summary>
        /// Allows a user to specify the HTTP Client to be used for the request, rather than using the build in settings.
        /// </summary>
        /// <param name="client"></param>
        /// <returns>The GivenContext representing the setup of the request.</returns>
        GivenContext HttpClient(HttpClient client);

        /// <summary>
        /// Allows a user to set the name of the current test suite.
        /// </summary>
        /// <param name="name">The name to apply to the current test suite.</param>
        /// <returns>The GivenContext object with the suite name set.</returns>
        GivenContext Name(string name);

        /// <summary>
        /// Allows a user to add a single query parameter to a request.
        /// </summary>
        /// <param name="key">The key of the query parameter.</param>
        /// <param name="value">The value of the query parameter to use.</param>
        /// <returns>The GivenContext representing the setup of the request.</returns>
        GivenContext Parameter(string key, string value);

        /// <summary>
        /// Allows a user to add a collection of query parameters to a request.
        /// </summary>
        /// <param name="parameters">The dictionary representing the query parameters for the request.</param>
        /// <returns>The GivenContext representing the setup of the request.</returns>
        GivenContext Parameters(Dictionary<string, string> parameters);

        /// <summary>
        /// Allows a user to set the name of the port on the host to use for the test.
        /// </summary>
        /// <param name="port">The port number to use within the test.</param>
        /// <returns>The GivenContext object with the port number set.</returns>
        GivenContext Port(int port);

        /// <summary>
        /// Adds a proxy using the user's AD account to access sites through a secure proxy
        /// </summary>
        /// <param name="address">The address of the proxy.</param>
        /// <returns>The GivenContext object with the suite name set.</returns>
        GivenContext Proxy(string address);

        /// <summary>
        /// Adds a proxy using a username and password to access sites through a secure proxy
        /// </summary>
        /// <param name="address">The address of the proxy.</param>
        /// <param name="userName">The username for the proxy server</param>
        /// <param name="password">The password for the proxy server</param>
        /// <returns>The GivenContext object with the suite name set.</returns>
        GivenContext Proxy(string address, string userName, string password);

        /// <summary>
        /// Allows a user to add a series of query strings to a request.
        /// </summary>
        /// <param name="queries">A collection of query strings.</param>
        /// <returns>The GivenContext representing the setup of the request.</returns>
        GivenContext Queries(List<QueryParams> queries);

        /// <summary>
        /// Allows a user to add a single query string to a request.
        /// </summary>
        /// <param name="key">The key of the query.</param>
        /// <param name="value">The value of the query string to use.</param>
        /// <returns>The GivenContext representing the setup of the request.</returns>
        GivenContext Query(string key, string value);

        /// <summary>
        /// Allows the user to set a timeout for the HTTP request.
        /// </summary>
        /// <param name="milliseconds">The number of milliseconds to wait.</param>
        /// <returns>The GivenContext object with the timeout set.</returns>
        GivenContext Timeout(int milliseconds);

        /// <summary>
        /// Allows a user to set the name of the URI of the endpoint itself.
        /// </summary>
        /// <param name="uri">The URI to use for the endpoint.</param>
        /// <returns>The GivenContext object with the endpoint URI set.</returns>
        GivenContext Uri(string uri);

        /// <summary>
        /// Used to initialise the WhenContext
        /// </summary>
        /// <returns>The WhenContext being used.</returns>
        WhenContext When();
    }
}