using System;
using System.Collections.Generic;
using System.Net;

namespace Liberator.RESTore
{
    public interface IResultContext
    {
        Dictionary<string, bool> Assertions { get; set; }
        string Content { get; set; }
        TimeSpan ElapsedExecutionTime { get; set; }
        Dictionary<string, IEnumerable<string>> Headers { get; set; }
        bool IsSuccessStatus { get; set; }
        List<LoadResponse> LoadResponses { get; set; }
        Dictionary<string, double> LoadValues { get; set; }
        HttpStatusCode StatusCode { get; set; }

        ThenContext AssertBody(Func<string, bool> assert);
        ThenContext AssertBody<TContent>(Func<TContent, bool> assert);
        ThenContext AssertFailureStatus();
        ThenContext AssertHeader(string headerType, Func<IEnumerable<string>, bool> assert);
        ThenContext AssertHeader(string headerType, string value);
        ThenContext AssertHeaders(Dictionary<string, string> headers);
        ThenContext AssertPass();
        ThenContext AssertStatus(Func<int, bool> assert);
        ThenContext AssertStatus(HttpStatusCode httpStatusCode);
        ThenContext AssertStatus(int statusCode);
        ThenContext AssertSuccessStatus();
        ThenContext AssessBody(string testName, Func<string, bool> assert);
        ThenContext AssessBody<TContent>(string testName, Func<TContent, bool> assert);
        ThenContext AssessFailureStatus();
        ThenContext AssessHeader(string headerType, string value);
        ThenContext AssessHeader(string testName, string headerType, Func<IEnumerable<string>, bool> assert);
        ThenContext AssessHeaders(Dictionary<string, string> headers);
        ThenContext AssessStatus(HttpStatusCode httpStatusCode);
        ThenContext AssessStatus(int statusCode);
        ThenContext AssessStatus(string testName, Func<int, bool> assert);
        ThenContext AssessSuccessStatus();
        ThenContext GetContent(out string content);
        ThenContext GetJsonObject<TContent>(out TContent result);
        ThenContext GetXmlObject<TContent>(out TContent result);
    }
}