using Newtonsoft.Json;
using RESTore.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace RESTore
{
    public class GivenContext
    {
        public string SuiteName { get; set; }
        public string HostName { get; set; }
        public int HostPort { get; set; }
        public string TargetUri { get; set; }
        public TimeSpan RequestTimeout { get; set; }
        public string RequestBody { get; set; }
        public List<FileContent> Files { get; set; }
        public Dictionary<string, string> SiteCookies { get; set; }
        public Dictionary<string, string> RequestHeaders { get; set; }
        public bool SecureHttp { get; set; }
        public HttpClient Client { get; set; }
        public Dictionary<string, string> QueryStrings { get; set; }
        public Dictionary<string, string> QueryParameters { get; set; }
        
        public GivenContext()
        {
            Client = new HttpClient();
            Files = new List<FileContent>();
            SiteCookies = new Dictionary<string, string>();
            RequestHeaders = new Dictionary<string, string>();
            QueryStrings = new Dictionary<string, string>();
            QueryParameters = new Dictionary<string, string>();
            RequestTimeout = new TimeSpan(0, 0, 0, 30, 0);
        }


        /// <summary>
        /// Allows a user to set the name of the current test suite.
        /// </summary>
        /// <param name="name">The name to apply to the current test suite.</param>
        /// <returns>The GivenContext object with the suite name set.</returns>
        public GivenContext Name(string name)
        {
            SuiteName = name;
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
            RequestBody = JsonConvert.SerializeObject(body);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="contentDispositionName"></param>
        /// <param name="contentType"></param>
        /// <param name="content"></param>
        /// <returns></returns>
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

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public GivenContext Cookie(string name, string value)
        {
            if (!SiteCookies.ContainsKey(name))
            {
                SiteCookies.Add(name, value);
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public GivenContext Cookies(Dictionary<string, string> cookies)
        {
            foreach (var cookie in cookies)
            {
                if (!SiteCookies.ContainsKey(cookie.Key))
                {
                    SiteCookies.Add(cookie.Key, cookie.Value);
                }
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public GivenContext Headers(Dictionary<string, string> headers)
        {
            foreach (var header in headers)
            {
                if (!RequestHeaders.ContainsKey(header.Key))
                    RequestHeaders.Add(header.Key, header.Value);
            }
            return this;
        }


        public GivenContext Header(string headerType, string value)
        {
            if (!RequestHeaders.ContainsKey(headerType))
                RequestHeaders.Add(headerType, value);

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public GivenContext Parameter(string key, string value)
        {
            if (!QueryParameters.ContainsKey(key))
                QueryParameters.Add(key, value);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public GivenContext Parameters(Dictionary<string, string> parameters)
        {
            foreach (var parameter in parameters)
            {
                if (!QueryParameters.ContainsKey(parameter.Key))
                    QueryParameters.Add(parameter.Key, parameter.Value);
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public GivenContext Query(string key, string value)
        {
            if (!QueryStrings.ContainsKey(key))
                QueryStrings.Add(key, value);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queries"></param>
        /// <returns></returns>
        public GivenContext Queries(Dictionary<string, string> queries)
        {
            foreach (var query in queries)
            {
                if (!QueryStrings.ContainsKey(query.Key))
                    QueryStrings.Add(query.Key, query.Value);
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public GivenContext HttpClient(HttpClient client)
        {
            Client = client;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public GivenContext UseHttps()
        {
            SecureHttp = true;
            return this;
        }

        /// <summary>
		/// Return all headers.
		/// </summary>
		/// <returns></returns>
		public Dictionary<string, string> Headers()
        {
            return RequestHeaders.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Return the value for a content-type header.
        /// </summary>
        /// <returns></returns>
        public string HeaderContentType()
        {
            return RequestHeaders[HeaderType.ContentType];
        }

        /// <summary>
        /// Return the value for an accept header.
        /// </summary>
        /// <returns></returns>
        public string HeaderAccept()
        {
            return RequestHeaders[HeaderType.Accept];
        }

        /// <summary>
        /// Return the value for an accept-encoding header.
        /// </summary>
        /// <returns></returns>
        public string HeaderAcceptEncoding()
        {
            return RequestHeaders[HeaderType.AcceptEncoding];
        }

        /// <summary>
        /// Return the value for an accept-charset header.
        /// </summary>
        /// <returns></returns>
        public string HeaderAcceptCharset()
        {
            return RequestHeaders[HeaderType.AcceptCharset];
        }

        /// <summary>
        /// Return all headers except for content-type, accept, accept-encoding and accept-charset
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> OtherHeaders()
        {
            return RequestHeaders
                .Where(
                x => x.Key != HeaderType.ContentType
                && x.Key != HeaderType.Accept
                && x.Key != HeaderType.AcceptEncoding
                && x.Key != HeaderType.AcceptCharset
                ).ToDictionary(x => x.Key, x => x.Value);
        }


        public WhenContext When()
        {
            return new WhenContext(this);
        }

    }
}
