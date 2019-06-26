using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Liberator.RESTore.Models
{
    public class GivenParameters
    {
        /// <summary>
        /// HTTP Client for the request.
        /// </summary>
        public HttpClient Client { get; set; }

        /// <summary>
        /// The files applied to the request.
        /// </summary>
        public List<FileContent> Files { get; set; }

        /// <summary>
        /// The name of the host.
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// The port to use on the host.
        /// </summary>
        public int HostPort { get; set; }

        /// <summary>
        /// The address of the proxy being used
        /// </summary>
        public string ProxyAddress { get; set; }

        /// <summary>
        /// The Form Parameters for the request.
        /// </summary>
        public List<KeyValuePair<string, object>> FormParameters { get; set; }

        /// <summary>
        /// The Query Strings for the request.
        /// </summary>
        public List<KeyValuePair<string, string>> QueryStrings { get; set; }

        /// <summary>
        /// The body of the request.
        /// </summary>
        public string RequestBody { get; set; }

        /// <summary>
        /// Headers for the request.
        /// </summary>
        public Dictionary<string, string> RequestHeaders { get; set; }

        /// <summary>
        /// The timeout for the request.
        /// </summary>
        public TimeSpan RequestTimeout { get; set; }

        /// <summary>
        /// Cookies for the request.
        /// </summary>
        public Dictionary<string, string> SiteCookies { get; set; }

        /// <summary>
        /// The name of the suite.
        /// </summary>
        public string SuiteName { get; set; }

        /// <summary>
        /// The target URL for the request.
        /// </summary>
        public string TargetUri { get; set; }
    }
}
