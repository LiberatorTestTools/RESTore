using RESTore.Enumerations;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace RESTore
{
    public class ExecutionContext
    {
        private readonly GivenContext _givenContext;
        private readonly WhenContext _whenContext;
        private readonly HttpClient _httpClient;
        private ConcurrentQueue<LoadResponse> _loadReponses = new ConcurrentQueue<LoadResponse>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setupContext"></param>
        /// <param name="whenContext"></param>
        public ExecutionContext(GivenContext setupContext, WhenContext whenContext)
        {
            _givenContext = setupContext;
            _whenContext = whenContext;

            _httpClient = _givenContext.Client;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ThenContext Then()
        {
            if (_whenContext.IsLoadTest)
            {
                StartCallsForLoad();
            }

            // var response = AsyncContext.Run(async () => await ExecuteCall());
            var response = ExecuteCall().GetAwaiter().GetResult();
            return BuildFromResponse(response);
        }

        #region HttpAction Strategy

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <returns></returns>
        private HttpRequestMessage BuildPost()
        {
            var request = new HttpRequestMessage()
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
        /// 
        /// </summary>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="request"></param>
        private void AppendHeaders(HttpRequestMessage request)
        {
            if (_givenContext.RequestHeaders.IsPresentInDictionary("Accept"))
            {
                _httpClient.DefaultRequestHeaders.Add("Accept", _givenContext.HeaderAccept());
            }
            if (_givenContext.RequestHeaders.IsPresentInDictionary("Accept Encoding"))
            {
                _httpClient.DefaultRequestHeaders.Add("Accept Encoding", _givenContext.HeaderAccept());
            }
            if (_givenContext.RequestHeaders.IsPresentInDictionary("Accept Charset"))
            {
                _httpClient.DefaultRequestHeaders.Add("Accept Charset", _givenContext.HeaderAcceptCharset());
            }
            if (_givenContext.RequestHeaders.IsPresentInDictionary("Content Type"))
            {
                _httpClient.DefaultRequestHeaders.Add("Content Type", _givenContext.HeaderAcceptCharset());
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
        /// 
        /// </summary>
        /// <param name="request"></param>
        private void AppendCookies(HttpRequestMessage request)
        {
            request.Headers.Add("Cookie", string.Join(";", _givenContext.SiteCookies.Select(x => x.Key + "=" + x.Value)));
        }

        /// <summary>
        /// 
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private HttpContent BuildContent()
        {
            if (_givenContext.Files.Any())
                return BuildMultipartContent();
            if (_givenContext.QueryParameters.Any())
                return BuildFormContent();
            if (!string.IsNullOrEmpty(_givenContext.RequestBody))
                return BuildStringContent();

            return null;
        }

        private HttpContent BuildMultipartContent()
        {
            var content = new MultipartFormDataContent();

            foreach (var pair in _givenContext.QueryParameters)
            {
                content.Add(new StringContent(pair.Value), pair.Key.Quote());
            }

            foreach (var item in _givenContext.Files)
            {

            }

            _givenContext.Files.ForEach(x =>
            {
                var fileContent = new ByteArrayContent(x.Content);
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    Name = x.ContentDispositionName.Quote(),
                    FileName = x.FileName
                };
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(x.ContentType);

                content.Add(fileContent);
            });

            return content;
        }

        private HttpContent BuildFormContent()
        {
            return new FormUrlEncodedContent(
                _givenContext.QueryParameters.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToList());
        }

        private HttpContent BuildStringContent()
        {
            return new StringContent(_givenContext.RequestBody, Encoding.UTF8,
                _givenContext.HeaderContentType());
        }

        #endregion
        
        #region Load Test Code

        public void StartCallsForLoad()
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

            // swapping this out
            // AsyncContext.Run(async () => await Task.WhenAll(taskThreads));
            Task.WhenAll(taskThreads).GetAwaiter().GetResult();
        }

        public async Task SingleThread(CancellationToken cancellationToken)
        {
            do
            {
                await MapCall();
            } while (!cancellationToken.IsCancellationRequested);
        }

        public async Task MapCall()
        {
            var loadResponse = new LoadResponse()
            { StatusCode = -1, Ticks = -1 };
            _loadReponses.Enqueue(loadResponse);

            var result = await ExecuteCall();
            loadResponse.StatusCode = (int)result.Response.StatusCode;
            loadResponse.Ticks = result.TimeElapsed.Ticks;
        }

        private async Task<TimedResponse> ExecuteCall()
        {
            var watch = new Stopwatch();
            watch.Start();
            var response = await _httpClient.SendAsync(BuildRequest());
            watch.Stop();
            return new TimedResponse {

                TimeElapsed = watch.Elapsed,
                Response = response
            };
        }

        private ThenContext BuildFromResponse(TimedResponse result)
        {
            // var content = AsyncContext.Run(async () => await result.Response.Content.ReadAsStringAsync());
            var content = result.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return new ThenContext()
            {
                StatusCode = result.Response.StatusCode,
                Content = content,
                Headers = result.Response.Content.Headers.ToDictionary(x => x.Key.Trim(), x => x.Value),
                ElapsedExecutionTime = result.TimeElapsed,
                LoadResponses = _loadReponses.ToList()
                };

        }

        #endregion
    }
}
