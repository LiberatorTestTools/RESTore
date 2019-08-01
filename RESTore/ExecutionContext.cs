﻿// This version, Copyright 2019 Liberator Test Tools
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
using Liberator.RESTore.Extensions;
using Liberator.RESTore.MIME;
using Liberator.RESTore.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Liberator.RESTore
{
    /// <summary>
    /// Represents the executing request
    /// </summary>
    public class ExecutionContext : IExecutionContext
    {
        #region Private Properties

        /// <summary>
        /// Represents the setup of the request.
        /// </summary>
        internal readonly GivenContext _givenContext;

        /// <summary>
        /// Represents the action to be executed.
        /// </summary>
        internal readonly WhenContext _whenContext;

        /// <summary>
        /// The HTTP Client to use for the request.
        /// </summary>
        internal HttpClient _httpClient;

        /// <summary>
        /// The compiled responses from the load test.
        /// </summary>
        internal ConcurrentQueue<LoadResponse> _loadReponses = new ConcurrentQueue<LoadResponse>();

        public Encoding MessageEncoding { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// The constructor for the ThenContext
        /// </summary>
        /// <param name="givenContext">The setup to use for the reques.</param>
        /// <param name="whenContext">The action to use for the request.</param>
        public ExecutionContext(GivenContext givenContext, WhenContext whenContext)
        {
            _givenContext = givenContext;
            _whenContext = whenContext;

            _httpClient = whenContext.GivenContext.Client;

        }

        #endregion

        #region ThenContext Initialiser

        /// <summary>
        /// Initialises the sending of the request.
        /// </summary>
        /// <returns>The built response from the server.</returns>
        public ThenContext Then()
        {
            if (_whenContext.IsLoadTest)
            {
                StartCallsForLoad();
            }

            var response = ExecuteCall().GetAwaiter().GetResult();
            return BuildFromResponse(response);
        }

        #endregion

        #region HttpAction Strategy

        /// <summary>
        /// Builds a request based on the verb used.
        /// </summary>
        /// <returns>The HTTP Request object representing the request.</returns>
        private HttpRequestMessage BuildRequest()
        {
            switch (_whenContext.HttpVerbUsed)
            {
                case HTTPVerb.GET:
                    return BuildGet();
                case HTTPVerb.POST:
                    return BuildPost();
                case HTTPVerb.PUT:
                    return BuildPut();
                case HTTPVerb.PATCH:
                    return BuildPatch();
                case HTTPVerb.DELETE:
                    return BuildDelete();
                default:
                    throw new Exception("No functionality is available for that Verb.");
            }
        }

        /// <summary>
        /// Builds a GET request.
        /// </summary>
        /// <returns>The HTTP Request object representing the GET request.</returns>
        private HttpRequestMessage BuildGet()
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = BuildUri(),
                Method = HttpMethod.Get
            };

            AppendHeaders(request);
            AppendCookies(request);
            SetTimeout();

            return request;
        }

        /// <summary>
        /// Builds a POST request.
        /// </summary>
        /// <returns>The HTTP Request object representing the POST request.</returns>
        private HttpRequestMessage BuildPost()
        {
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = BuildUri(),
                Method = HttpMethod.Post
            };
            AppendHeaders(request);
            AppendCookies(request);
            SetTimeout();

            request.Content = BuildContent();

            return request;

        }


        /// <summary>
        /// Builds a PUT request.
        /// </summary>
        /// <returns>The HTTP Request object representing the PUT request.</returns>
        private HttpRequestMessage BuildPut()
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = BuildUri(),
                Method = HttpMethod.Put
            };

            AppendHeaders(request);
            AppendCookies(request);
            SetTimeout();

            request.Content = BuildContent();

            return request;
        }

        /// <summary>
        /// Builds a PATCH request
        /// </summary>
        /// <returns>The HTTP Request object representing the PATCH request.</returns>
        private HttpRequestMessage BuildPatch()
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = BuildUri(),
                Method = new HttpMethod("PATCH")
            };

            AppendHeaders(request);
            AppendCookies(request);
            SetTimeout();

            request.Content = BuildContent();

            return request;
        }

        /// <summary>
        /// Builds a DELETE request.
        /// </summary>
        /// <returns>The HTTP Request object representing the DELETE request.</returns>
        private HttpRequestMessage BuildDelete()
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = BuildUri(),
                Method = HttpMethod.Delete
            };

            AppendHeaders(request);
            AppendCookies(request);
            SetTimeout();

            request.Content = BuildContent();

            return request;
        }

        /// <summary>
        /// Builds the URL for the request.
        /// </summary>
        /// <returns>The URI used in the request.</returns>
        private Uri BuildUri()
        {
            var builder = new UriBuilder(_whenContext.TargetUrl);
            var query = HttpUtility.ParseQueryString(builder.Query);

            foreach (var queryString in _givenContext.QueryStrings)
            {
                query.Add(queryString.Key, queryString.Value);
            }

            builder.Query = query.ToString();
            return new Uri(builder.ToString());
        }

        /// <summary>
        /// Adds the request headers to the default request headers collection
        /// </summary>
        /// <param name="request">The request to be sent to the endpoint.</param>
        private void AppendHeaders(HttpRequestMessage request)
        {
            if (_givenContext.RequestHeaders.IsPresentInDictionary(HeaderType.Accept))
            {
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_givenContext.HeaderAccept()));
            }
            if (_givenContext.RequestHeaders.IsPresentInDictionary(HeaderType.AcceptEncoding))
            {
                _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue(_givenContext.HeaderAcceptEncoding()));
            }
            if (_givenContext.RequestHeaders.IsPresentInDictionary(HeaderType.AcceptCharset))
            {
                _httpClient.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue(_givenContext.HeaderAcceptCharset()));
            }
            if (_givenContext.OtherHeaders().Count > 0)
            {
                foreach (KeyValuePair<string, string> header in _givenContext.OtherHeaders())
                {
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);

                }
            }
        }

        /// <summary>
        /// Appends cookies to the request.
        /// </summary>
        /// <param name="request">The request to be sent to the endpoint.</param>
        private void AppendCookies(HttpRequestMessage request)
        {
            if (_givenContext.SiteCookies.Count != 0)
                request.Headers.Add("Cookie", string.Join(";", _givenContext.SiteCookies.Select(x => x.Key + "=" + x.Value)));
        }

        /// <summary>
        /// Sets the timeout for the HTTP requests.
        /// </summary>
        private void SetTimeout()
        {
            if (_givenContext.RequestTimeout != null)
            {
                _httpClient.Timeout = new TimeSpan(_givenContext.RequestTimeout.Ticks);
            }
        }

        #endregion

        #region Content Buildup Strategy

        internal HttpContent BuildContent()
        {
            if (_givenContext.FormParameters.Count > 0 || _givenContext.SubmittedFiles.Count > 0)
            {
                return BuildMultipartFormData();
            }
            else if (_givenContext.RequestBody.Length > 0)
            {
                return BuildRequestBody();
            }
            return null;
        }

        internal MultipartFormDataContent BuildMultipartFormData()
        {
            using (MultipartFormDataContent formContent = new MultipartFormDataContent())
            {
                FormUrlEncodedContent formData = new FormUrlEncodedContent(_givenContext.FormParameters.Cast<KeyValuePair<string, string>>());
                formContent.Headers.ContentType.MediaType = Multipart.FormData;
                formContent.Add(formData);

                for (int i = 0; i < _givenContext.SubmittedFiles.Count; i++)
                {
                    string fileName = _givenContext.SubmittedFiles[i].FileName;
                    Stream fileStream = File.OpenRead(fileName);
                    formContent.Add(new StreamContent(fileStream), fileName, fileName);
                }
                return formContent;
            }
        }

        internal HttpContent BuildRequestBody()
        {
            return new StringContent(_givenContext.RequestBody);
        }

        #endregion

        #region Load Test Code

        /// <summary>
        /// Starts the load test element of the request.
        /// </summary>
        internal void StartCallsForLoad()
        {
            ServicePointManager.DefaultConnectionLimit = _whenContext.Threads;

            var cancellationTokenSource = new CancellationTokenSource();

            var taskThreads = new List<Task>();
            for (var i = 0; i < _whenContext.Threads; i++)
            {
                taskThreads.Add(Task.Run(async () =>
                {
                    await SingleThread(cancellationTokenSource.Token);
                }, cancellationTokenSource.Token));
            }

            Timer timer = null;
            timer = new Timer((ignore) =>
            {
                timer.Dispose();
                cancellationTokenSource.Cancel();
            }, null, TimeSpan.FromSeconds(_whenContext.Seconds), TimeSpan.FromMilliseconds(-1));

            Task.WhenAll(taskThreads).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Creates a single thread for load testing
        /// </summary>
        /// <param name="cancellationToken">The type of token used to cancel the request.</param>
        /// <returns>The underlying task object.</returns>
        internal async Task SingleThread(CancellationToken cancellationToken)
        {
            do
            {
                await MapCall();
            } while (!cancellationToken.IsCancellationRequested);
        }

        /// <summary>
        /// Maps the results of a load test call to a load response object.
        /// </summary>
        /// <returns>The underlying task object.</returns>
        internal async Task MapCall()
        {
            var loadResponse = new LoadResponse()
            { StatusCode = -1, Ticks = -1 };
            _loadReponses.Enqueue(loadResponse);

            var result = await ExecuteCall();
            loadResponse.StatusCode = (int)result.Response.StatusCode;
            loadResponse.Ticks = result.TimeElapsed.Ticks;
        }

        /// <summary>
        /// Executes the call to the host.
        /// </summary>
        /// <returns>A task representing the executed call.</returns>
        private async Task<TimedResponse> ExecuteCall()
        {
            var watch = new Stopwatch();
            watch.Start();
            HttpResponseMessage response = await _httpClient.SendAsync(BuildRequest());
            watch.Stop();
            return new TimedResponse
            {
                TimeElapsed = watch.Elapsed,
                Response = response
            };
        }

        /// <summary>
        /// Populates the context with the values from the response.
        /// </summary>
        /// <param name="result">The result from host.</param>
        /// <returns>The ThenConext object representing the result.</returns>
        private ThenContext BuildFromResponse(TimedResponse result)
        {
            var content = result.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            var contentHeaders = result.Response.Content.Headers;
            var headers = result.Response.Headers;

            ThenContext thenContext = new ThenContext()
            {
                Content = content,
                Headers = contentHeaders.Concat(headers).ToDictionary(entry => entry.Key.Trim(), entry => entry.Value),
                IsSuccessStatus = result.Response.IsSuccessStatusCode,
                StatusCode = result.Response.StatusCode,
                ElapsedExecutionTime = result.TimeElapsed,
                LoadResponses = _loadReponses.ToList()
            };
            return thenContext;
        }

        #endregion
    }
}
/* 

 */
