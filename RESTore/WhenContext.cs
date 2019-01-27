using RESTore.Enumerations;
using System;
using System.Runtime.InteropServices;

namespace RESTore
{
    public class WhenContext
    {
        public bool IsLoadTest { get; set; }
        public HTTPVerb HttpVerbUsed { get; set; }
        public WhenContext CurrentWhenContext { get; set; }

        public GivenContext GivenContext { get; set; }
        public string TargetUrl { get; set; }
        public int Threads { get; set; }
        public int Seconds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="givenContext"></param>
        public WhenContext(GivenContext givenContext)
        {
            GivenContext = givenContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="threads"></param>
        /// <returns></returns>
        public WhenContext LoadTest([Optional, DefaultParameterValue(60)]int seconds, [Optional, DefaultParameterValue(1)]int threads)
        {
            IsLoadTest = true;
            Threads = threads < 0 ? 1 : threads;
            Seconds = seconds < 0 ? 60 : seconds;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ExecutionContext Get([Optional, DefaultParameterValue(null)]string url)
        {
            return SetHttpAction(url, HTTPVerb.GET);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ExecutionContext Post([Optional, DefaultParameterValue(null)]string url)
        {
            return SetHttpAction(url, HTTPVerb.POST);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ExecutionContext Put([Optional, DefaultParameterValue(null)]string url)
        {
            return SetHttpAction(url, HTTPVerb.PUT);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ExecutionContext Patch([Optional, DefaultParameterValue(null)]string url)
        {
            return SetHttpAction(url, HTTPVerb.PATCH);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ExecutionContext Delete([Optional, DefaultParameterValue(null)]string url)
        {
            return SetHttpAction(url, HTTPVerb.DELETE);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string SetUrl(string url)
        {
            if (string.IsNullOrEmpty(url) && string.IsNullOrEmpty(GivenContext.HostName))
            {
                throw new ArgumentException("URL must be provided");
            }

            var uri = !string.IsNullOrEmpty(url)
                ? new Uri(url.FixProtocol(GivenContext.SecureHttp))
                : new Uri(new Uri(GivenContext.HostName.FixProtocol(GivenContext.SecureHttp)), GivenContext.TargetUri);

            TargetUrl = uri.OriginalString;
            return TargetUrl;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="httpVerb"></param>
        /// <returns></returns>
        private ExecutionContext SetHttpAction(string url, HTTPVerb httpVerb)
        {
            SetUrl(url);
            HttpVerbUsed = httpVerb;
            return new ExecutionContext(GivenContext, this);
        }
    }
}
