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


using Liberator.RESTore.Enumerations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Xml;
using System.Xml.Serialization;

namespace Liberator.RESTore
{
    /// <summary>
    /// The setup for the request.
    /// </summary>
    public class GivenContext
    {
        #region Public Properties

        /// <summary>
        /// The name of the suite.
        /// </summary>
        public string SuiteName { get; set; }

        /// <summary>
        /// The name of the host.
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// The port to use on the host.
        /// </summary>
        public int HostPort { get; set; }

        /// <summary>
        /// The target URL for the request.
        /// </summary>
        public string TargetUri { get; set; }

        /// <summary>
        /// The timeout for the request.
        /// </summary>
        public TimeSpan RequestTimeout { get; set; }

        /// <summary>
        /// The body of the request.
        /// </summary>
        public string RequestBody { get; set; }

        /// <summary>
        /// The address of the proxy being used
        /// </summary>
        public string ProxyAddress { get; set; }

        /// <summary>
        /// The files applied to the request.
        /// </summary>
        public List<FileContent> Files { get; set; }

        /// <summary>
        /// Cookies for the request.
        /// </summary>
        public Dictionary<string, string> SiteCookies { get; set; }

        /// <summary>
        /// Headers for the request.
        /// </summary>
        public Dictionary<string, string> RequestHeaders { get; set; }

        /// <summary>
        /// Whether to use a secure HTTP connection
        /// </summary>
        public bool SecureHttp { get; set; }

        /// <summary>
        /// HTTP Client for the request.
        /// </summary>
        public HttpClient Client { get; set; }

        /// <summary>
        /// The Query Strings for the request.
        /// </summary>
        public Dictionary<string, string> QueryStrings { get; set; }

        /// <summary>
        /// The Query Parameters for the request.
        /// </summary>
        public Dictionary<string, string> QueryParameters { get; set; }

        #endregion

        #region Constructor
    
        /// <summary>
        /// The GivenContext
        /// </summary>
        public GivenContext()
        {
            RESToreSettings.Log.WriteLine("--GIVEN--");
            Client = new HttpClient();
            Files = new List<FileContent>();
            SiteCookies = new Dictionary<string, string>();
            RequestHeaders = new Dictionary<string, string>();
            QueryStrings = new Dictionary<string, string>();
            QueryParameters = new Dictionary<string, string>();
            RequestTimeout = new TimeSpan(0, 0, 0, 30, 0);
        }

        #endregion

        #region Given Methods

        /// <summary>
        /// Allows a user to set the name of the current test suite.
        /// </summary>
        /// <param name="name">The name to apply to the current test suite.</param>
        /// <returns>The GivenContext object with the suite name set.</returns>
        public GivenContext Name(string name)
        {
            SuiteName = name;
            RESToreSettings.Log.WriteLine($"Test Suite Name: {SuiteName}");
            return this;
        }

        /// <summary>
        /// Adds a proxy using the user's AD account to access sites through a secure proxy
        /// </summary>
        /// <param name="address">The address of the proxy.</param>
        /// <returns>The GivenContext object with the suite name set.</returns>
        public GivenContext Proxy(string address)
        {
            Client = new HttpClient(AddProxyToClient(address));
            RESToreSettings.Log.WriteLine($"Added proxy: {address} using default credentials");
            return this;
        }

        /// <summary>
        /// Adds a proxy using a username and password to access sites through a secure proxy
        /// </summary>
        /// <param name="address">The address of the proxy.</param>
        /// <param name="userName">The username for the proxy server</param>
        /// <param name="password">The password for the proxy server</param>
        /// <returns>The GivenContext object with the suite name set.</returns>
        public GivenContext Proxy(string address, string userName, string password)
        {
            Client = new HttpClient(AddProxyToClient(address, userName, password));
            RESToreSettings.Log.WriteLine($"Added proxy: {address} using username: {userName} and password: {password}");
            return this;
        }



        /// <summary>
        /// Allows a user to set the name of the host to use for the test suite.
        /// </summary>
        /// <param name="host">The name of the host to use for the test suite.</param>
        /// <returns>The GivenContext object with the host name set.</returns>
        public GivenContext Host(string host)
        {
            HostName = host;
            RESToreSettings.Log.WriteLine($"Request will be sent to: {host}");
            return this;
        }

        /// <summary>
        /// Allows a user to set the name of the port on the host to use for the test.
        /// </summary>
        /// <param name="port">The port number to use within the test.</param>
        /// <returns>The GivenContext object with the port number set.</returns>
        public GivenContext Port(int port)
        {
            HostPort = port;
            RESToreSettings.Log.WriteLine($"Using Port: {port}");
            return this;
        }

        /// <summary>
        /// Calculates the fully qualified name for the host with optional port.
        /// </summary>
        /// <returns>The fully qualified name for the host.</returns>
        public string HostNameWithPort()
        {
            return HostPort > 0 && HostPort != 88 ? $"{HostName}:{HostPort}" : HostName;
        }

        /// <summary>
        /// Allows a user to set the name of the URI of the endpoint itself.
        /// </summary>
        /// <param name="uri">The URI to use for the endpoint.</param>
        /// <returns>The GivenContext object with the endpoint URI set.</returns>
        public GivenContext Uri(string uri)
        {
            TargetUri = uri;
            RESToreSettings.Log.WriteLine($"Hitting Endpoint: {uri}");
            return this;
        }

        /// <summary>
        /// Allows the user to set a timeout for the HTTP request.
        /// </summary>
        /// <param name="milliseconds">The number of milliseconds to wait.</param>
        /// <returns>The GivenContext object with the timeout set.</returns>
        public GivenContext Timeout(int milliseconds)
        {
            RequestTimeout = new TimeSpan(0, 0, 0, 0, milliseconds);
            RESToreSettings.Log.WriteLine($"Setting Request Timeout to {milliseconds}ms");
            return this;
        }

        /// <summary>
        /// Allows a user to set the body of the request.
        /// </summary>
        /// <param name="body">The body to be sent with the request.</param>
        /// <returns>The GivenContext object with the request body set.</returns>
        public GivenContext Body(string body)
        {
            RequestBody = body;
            RESToreSettings.Log.WriteLine($"Using Body: {body}");
            return this;
        }

        /// <summary>
        /// Allows a user to set the body of the request using an XML Document
        /// </summary>
        /// <param name="xmlDocument">Document object to be used in the call.</param>
        /// <returns>The GivenContext object with the request body set.</returns>
        public GivenContext Body(XmlDocument xmlDocument)
        {
            RequestBody = xmlDocument.ToString();
            RESToreSettings.Log.WriteLine($"Using Body: {xmlDocument}");
            return this;
        }

        /// <summary>
        /// Allows a user to add a body based on an Object and serialises it.
        /// </summary>
        /// <typeparam name="T">The type of object being passed.</typeparam>
        /// <param name="body">The body object.</param>
        /// <returns>The GivenContext object with the request body set.</returns>
        public GivenContext Body<T>(T body) where T : class
        {
            string bodyString = JsonConvert.SerializeObject(body);
            RequestBody = bodyString;
            RESToreSettings.Log.WriteLine($"Using Body: {bodyString}");
            return this;
        }

        /// <summary>
        /// Allows a user to add a body based on an Object and serialises it.
        /// </summary>
        /// <typeparam name="T">The type of object being passed.</typeparam>
        /// <param name="body">The body object.</param>
        /// <returns>The GivenContext object with the request body set.</returns>
        public GivenContext BodyXML<T>(T body) where T : class
        {
            string bodyString = null;
            XmlSerializer xmlSerializer = new XmlSerializer(body.GetType());
            using (StringWriter stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, body);
                bodyString = stringWriter.ToString();
            }
            RequestBody = bodyString;
            RESToreSettings.Log.WriteLine($"Using Body: {bodyString}");
            return this;
        }

        /// <summary>
        /// Allows a user to add a file to the request.
        /// </summary>
        /// <param name="fileName">The name of the file to add.</param>
        /// <param name="contentDispositionName">A header that indicates if the file is to displayed inline in the browser.</param>
        /// <param name="contentType">The content-type of the file.</param>
        /// <param name="content">The content of the file as a byte array.</param>
        /// <returns>The GivenContext representing the setup of the request.</returns>
        public GivenContext File(string fileName, string contentDispositionName, string contentType, byte[] content)
        {
            Files.Add(
                new FileContent()
                {
                    FileName = fileName,
                    ContentDispositionName = contentDispositionName,
                    ContentType = contentType,
                    Content = content
                });
            RESToreSettings.Log.WriteLine($"Uploading file: {fileName}");
            return this;
        }

        /// <summary>
        /// Allows a user to add a single cookie to a request.
        /// </summary>
        /// <param name="name">The name of the cookie parameter.</param>
        /// <param name="value">The value of the cookie parameter.</param>
        /// <returns>The GivenContext representing the setup of the request.</returns>
        public GivenContext Cookie(string name, string value)
        {
            if (!SiteCookies.ContainsKey(name))
            {
                SiteCookies.Add(name, value);
                RESToreSettings.Log.WriteLine($"Adding cookie: {name} with value: {value}");
            }
            return this;
        }

        /// <summary>
        /// Allows a user to add a collection of cookies to a request.
        /// </summary>
        /// <param name="cookies">The dictionary representing the cookies for the request.</param>
        /// <returns>The GivenContext representing the setup of the request.</returns>
        public GivenContext Cookies(Dictionary<string, string> cookies)
        {
            foreach (var cookie in cookies)
            {
                if (!SiteCookies.ContainsKey(cookie.Key))
                {
                    SiteCookies.Add(cookie.Key, cookie.Value);
                    RESToreSettings.Log.WriteLine($"Adding cookie: {cookie.Key} with value: {cookie.Value}");
                }
            }
            return this;
        }

        /// <summary>
        /// Allows a user to add a collection of headers to a request.
        /// </summary>
        /// <param name="headers">The dictionary representing the query parameters for the request.</param>
        /// <returns>The GivenContext representing the setup of the request.</returns>
        public GivenContext Headers(Dictionary<string, string> headers)
        {
            foreach (var header in headers)
            {
                if (!RequestHeaders.ContainsKey(header.Key))
                {
                    RequestHeaders.Add(header.Key, header.Value);
                    RESToreSettings.Log.WriteLine($"Adding header: {header.Key} with value: {header.Value}");
                }
            }
            return this;
        }

        /// <summary>
        /// Allows a user to add a single header to a request.
        /// </summary>
        /// <param name="headerType">The type of header to use.</param>
        /// <param name="value">The value to use for the header.</param>
        /// <returns>The GivenContext representing the setup of the request.</returns>
        public GivenContext Header(string headerType, string value)
        {
            if (!RequestHeaders.ContainsKey(headerType))
            {
                RequestHeaders.Add(headerType, value);
                RESToreSettings.Log.WriteLine($"Adding header: {headerType} with value: {value}");
            }

            return this;
        }

        /// <summary>
        /// Allows a user to add a single query parameter to a request.
        /// </summary>
        /// <param name="key">The key of the query parameter.</param>
        /// <param name="value">The value of the query parameter to use.</param>
        /// <returns>The GivenContext representing the setup of the request.</returns>
        public GivenContext Parameter(string key, string value)
        {
            if (!QueryParameters.ContainsKey(key))
            {
                QueryParameters.Add(key, value);
                RESToreSettings.Log.WriteLine($"Adding form parameter: {key} with value: {value}");
            }
            return this;
        }

        /// <summary>
        /// Allows a user to add a collection of query parameters to a request.
        /// </summary>
        /// <param name="parameters">The dictionary representing the query parameters for the request.</param>
        /// <returns>The GivenContext representing the setup of the request.</returns>
        public GivenContext Parameters(Dictionary<string, string> parameters)
        {
            foreach (var parameter in parameters)
            {
                if (!QueryParameters.ContainsKey(parameter.Key))
                {
                    QueryParameters.Add(parameter.Key, parameter.Value);
                    RESToreSettings.Log.WriteLine($"Adding form parameter: {parameter.Key} with value: {parameter.Value}");
                }
            }
            return this;
        }

        /// <summary>
        /// Allows a user to add a single query string to a request.
        /// </summary>
        /// <param name="key">The key of the query.</param>
        /// <param name="value">The value of the query string to use.</param>
        /// <returns>The GivenContext representing the setup of the request.</returns>
        public GivenContext Query(string key, string value)
        {
            if (!QueryStrings.ContainsKey(key))
            {
                QueryStrings.Add(key, value);
                RESToreSettings.Log.WriteLine($"Adding to query string: {key} with value: {value}");
            }
            return this;
        }

        /// <summary>
        /// Allows a user to add a series of query strings to a request.
        /// </summary>
        /// <param name="queries">A dictionary represnting the collection of query strings.</param>
        /// <returns>The GivenContext representing the setup of the request.</returns>
        public GivenContext Queries(Dictionary<string, string> queries)
        {
            foreach (var query in queries)
            {
                if (!QueryStrings.ContainsKey(query.Key))
                {
                    QueryStrings.Add(query.Key, query.Value);
                    RESToreSettings.Log.WriteLine($"Adding to query string: {query.Key} with value: {query.Value}");
                }
            }
            return this;
        }

        /// <summary>
        /// Allows a user to specify the HTTP Client to be used for the request.
        /// </summary>
        /// <param name="client"></param>
        /// <returns>The GivenContext representing the setup of the request.</returns>
        public GivenContext HttpClient(HttpClient client)
        {
            Client = client;
            return this;
        }

        #endregion

        #region Headers

        /// <summary>
        /// Return all headers.
        /// </summary>
        /// <returns>A collection of headers as a dictionary.</returns>
        internal Dictionary<string, string> Headers()
        {
            return RequestHeaders.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Return the value for a content-type header.
        /// </summary>
        /// <returns>The content-type header for the request.</returns>
        internal string HeaderContentType()
        {
            return RequestHeaders[HeaderType.ContentType];
        }

        /// <summary>
        /// Return the value for an accept header.
        /// </summary>
        /// <returns>The accept header for the request</returns>
        internal string HeaderAccept()
        {
            return RequestHeaders[HeaderType.Accept];
        }

        /// <summary>
        /// Return the value for an accept-encoding header.
        /// </summary>
        /// <returns>The accept-encoding header for the request.</returns>
        internal string HeaderAcceptEncoding()
        {
            return RequestHeaders[HeaderType.AcceptEncoding];
        }

        /// <summary>
        /// Return the value for an accept-charset header.
        /// </summary>
        /// <returns>The accept-charset header for the request.</returns>
        internal string HeaderAcceptCharset()
        {
            return RequestHeaders[HeaderType.AcceptCharset];
        }

        /// <summary>
        /// Return all headers except for content-type, accept, accept-encoding and accept-charset
        /// </summary>
        /// <returns>A dictionary representing other headers.</returns>
        internal Dictionary<string, string> OtherHeaders()
        {
            return RequestHeaders
                .Where(
                x => x.Key != HeaderType.ContentType
                && x.Key != HeaderType.Accept
                && x.Key != HeaderType.AcceptEncoding
                && x.Key != HeaderType.AcceptCharset
                ).ToDictionary(x => x.Key, x => x.Value);
        }

        #endregion

        #region When Context Initialiser

        /// <summary>
        /// Used to initialise the WhenCOntext
        /// </summary>
        /// <returns>The WhenContext being used.</returns>
        public WhenContext When()
        {
            return new WhenContext(this);
        } 

        #endregion

        #region Private Methods

        internal HttpClientHandler AddProxyToClient(string proxyAddress)
        {
            WebProxy webProxy = new WebProxy(proxyAddress, false)
            {
                UseDefaultCredentials = true
            };

            return new HttpClientHandler()
            {
                Proxy = webProxy,
                UseProxy = true
            };
        }

        internal HttpClientHandler AddProxyToClient(string proxyAddress, string userName, string password)
        {

            return new HttpClientHandler()
            {
                Proxy = new WebProxy(proxyAddress, true, null, new NetworkCredential(userName, password)),
                UseProxy = true
            };
        } 

        #endregion
    }
}
