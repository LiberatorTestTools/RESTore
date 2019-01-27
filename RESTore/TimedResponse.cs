using System;
using System.Net.Http;

namespace RESTore
{
    public class TimedResponse
    {
        public HttpResponseMessage Response { get; set; }
        public TimeSpan TimeElapsed { get; set; }
    }
}
