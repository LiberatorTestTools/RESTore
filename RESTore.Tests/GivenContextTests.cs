using NUnit.Framework;
using Liberator.RESTore.Tests.Objects;
using System.Collections.Generic;
using System.Net.Http;

namespace Liberator.RESTore.Tests
{
    [TestFixture]
    public class GivenContextTests
    {
        private readonly Dictionary<string, string> headers =
            new Dictionary<string, string>()
            {
                { "Accept", "application/json" },
                { "Content-Type", "application/json" },
                { "Accept-Encoding", "gzip" },
                { "Accept-Charset", "utf-8" }
            };

        public GivenContextTests()
        {

        }

        [Test]
        [Category("Given Context : Properties")]
        public void EnsureSuiteNamePopulatesProperty()
        {
            GivenContext context
                = new RESTore()
                .Given()
                .Name("Test suite name");

            Assert.That(context.SuiteName.Equals("Test suite name"), Is.True);
        }

        [Test]
        [Category("Given Context : Properties")]
        public void EnsureHostNamePopulatesProperty()
        {
            GivenContext context
                = new RESTore()
                .Given()
                .Host("http://www.somewebserver.com/");

            Assert.That(context.HostName.Equals("http://www.somewebserver.com/"), Is.True);
        }

        [Test]
        [Category("Given Context : Properties")]
        public void EnsureHostPortPopulatesProperty()
        {
            GivenContext context
                = new RESTore()
                .Given()
                .Port(8080);

            Assert.That(context.HostPort == 8080, Is.True);
        }

        [Test]
        [Category("Given Context : Properties")]
        public void EnsureHostWithPortConcatenates()
        {
            GivenContext context
                = new RESTore()
                .Given()
                .Host("http://www.somewebserver.com/")
                .Port(8080);

            Assert.That(context.HostNameWithPort().Equals("http://www.somewebserver.com/:8080"), Is.True);
        }

        [Test]
        [Category("Given Context : Properties")]
        public void EnsureUriPopulatesProperty()
        {
            GivenContext context
                = new RESTore()
                .Given()
                .Uri("api_endpoint");

            Assert.That(context.TargetUri.Equals("api_endpoint"), Is.True);
        }

        [Test]
        [Category("Given Context : Properties")]
        public void EnsureTimeoutPopulatesProperty()
        {
            GivenContext context
                = new RESTore()
                .Given()
                .Timeout(100000);

            Assert.That(context.RequestTimeout.Days == 0, Is.True);
            Assert.That(context.RequestTimeout.Hours == 0, Is.True);
            Assert.That(context.RequestTimeout.Minutes == 1, Is.True);
            Assert.That(context.RequestTimeout.Seconds == 40, Is.True);
            Assert.That(context.RequestTimeout.Milliseconds == 0, Is.True);
        }

        [Test]
        [Category("Given Context : Properties")]
        public void EnsureBodyPopulatesProperty()
        {
            GivenContext context
                = new RESTore()
                .Given()
                .Body("{Json:Blob}");

            Assert.That(context.RequestBody.Equals("{Json:Blob}"), Is.True);
        }

        [Test]
        [Category("Given Context : Properties")]
        public void EnsureBodyEntityPopulatesProperty()
        {
            GivenContext context
                = new RESTore()
                .Given()
                .Body(
                    new TestEntity()
                    {
                        PropertyA = "SettingA",
                        PropertyB = "SettingB"
                    }
                    );

            Assert.That(context.RequestBody.Equals("{\"PropertyA\":\"SettingA\",\"PropertyB\":\"SettingB\"}"), Is.True);
        }

        [Test]
        [Category("Given Context : Properties")]
        public void EnsureFilePopulatesProperty()
        {
            GivenContext context
                = new RESTore()
                .Given()
                .File("FileName", "ContentDispositionName", "ContentType", new byte[1]);

            Assert.That(context.Files[0].ContentType == "ContentType", Is.True);
            Assert.That(context.Files[0].ContentDispositionName == "ContentDispositionName", Is.True);
            Assert.That(context.Files[0].FileName == "FileName", Is.True);
            Assert.That(context.Files[0].Content.Length == 1, Is.True);
        }

        [Test]
        [Category("Given Context : Properties")]
        public void EnsureCookiePopulatesProperty()
        {
            GivenContext context
                = new RESTore()
                .Given()
                .Cookie("key", "value");

            Assert.That(context.SiteCookies["key"].Equals("value"), Is.True);
        }

        [Test]
        [Category("Given Context : Properties")]
        public void EnsureCookiesPopulatesProperty()
        {
            var cookies = new Dictionary<string, string>();
            cookies.Add("key", "value");

            GivenContext context
                = new RESTore()
                .Given()
                .Cookies(cookies);

            Assert.That(context.SiteCookies["key"].Equals("value"), Is.True);
        }

        [Test]
        [Category("Given Context : Properties")]
        public void EnsureHeadersAddedToProperty()
        {
            var headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");

            GivenContext context
                = new RESTore()
                .Given()
                .Headers(headers);

            Assert.That(context.RequestHeaders["Content-Type"].Equals("application/json"), Is.True);
        }

        [Test]
        [Category("Given Context : Properties")]
        public void EnsureParameterAddedToProperty()
        {
            GivenContext context
                            = new RESTore()
                            .Given()
                            .Parameter("id", "abc123");

            Assert.That(context.QueryParameters["id"].Equals("abc123"), Is.True);
        }

        [Test]
        [Category("Given Context : Properties")]
        public void EnsureParametersAddedToProperty()
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("id", "abc123");

            GivenContext context
                = new RESTore()
                .Given()
                .Parameters(parameters);

            Assert.That(context.QueryParameters["id"].Equals("abc123"), Is.True);
        }

        [Test]
        [Category("Given Context : Properties")]
        public void EnsureQueryAddedToProperty()
        {
            GivenContext context
                = new RESTore()
                .Given()
                .Query("id", "abc123");

            Assert.That(context.QueryStrings["id"].Equals("abc123"), Is.True);
        }

        [Test]
        [Category("Given Context : Properties")]
        public void EnsureQueriesAddedToProperty()
        {
            Dictionary<string, string> queries = new Dictionary<string, string>();
            queries.Add("id", "abc123");

            var context
                = new RESTore()
                .Given()
                .Queries(queries);

            Assert.That(context.QueryStrings["id"].Equals("abc123"), Is.True);
        }

        [Test]
        [Category("Given Context : Properties")]
        public void EnsureHttpClientAddedToProperty()
        {
            HttpClient httpClient = new HttpClient()
            {
                BaseAddress = new System.Uri("http://www.totallyratted.com"),
                Timeout = new System.TimeSpan(0, 0, 1, 0, 0)
            };

            GivenContext context
                = new RESTore()
                .Given()
                .HttpClient(httpClient);

            Assert.That(context.Client.BaseAddress.Equals("http://www.totallyratted.com"), Is.True);
        }

        [Test]
        [Category("Given Context : Properties")]
        public void EnsureUseHttpsAddedToProperty()
        {
            GivenContext context
                = new RESTore()
                .Given()
                .UseHttps();

            Assert.That(context.SecureHttp, Is.True);
        }

        [Test]
        [Category("Given Context : Properties")]
        public void CheckHeadersMethodReturnsCorrectly()
        {
            GivenContext context
                = new RESTore()
                .Given()
                .Headers(headers);

            Assert.That(context.RequestHeaders.Count == 4, Is.True);
        }

        [Test]
        [Category("Given Context : Methods")]
        public void CheckHeadersReturnsCorrectly()
        {
            GivenContext context
                = new RESTore()
                .Given()
                .Headers(headers);

            Assert.That(context.Headers().Count == 4, Is.True);
        }

        [Test]
        [Category("Given Context : Methods")]
        public void CheckHeaderContentTypeMethodReturnsCorrectly()
        {
            GivenContext context
                = new RESTore()
                .Given()
                .Headers(headers);

            Assert.That(context.HeaderContentType().Contains("application/json"), Is.True);
        }

        [Test]
        [Category("Given Context : Methods")]
        public void CheckHeaderAcceptMethodReturnsCorrectly()
        {
            GivenContext context
                = new RESTore()
                .Given()
                .Headers(headers);

            Assert.That(context.HeaderAccept().Contains("application/json"), Is.True);
        }

        [Test]
        [Category("Given Context : Methods")]
        public void CheckHeaderAcceptEncodingMethodReturnsCorrectly()
        {
            GivenContext context
                = new RESTore()
                .Given()
                .Headers(headers);

            Assert.That(context.HeaderAcceptEncoding().Contains("gzip"), Is.True);
        }

        [Test]
        [Category("Given Context : Methods")]
        public void CheckHeaderAcceptCharsetMethodReturnsCorrectly()
        {
            GivenContext context
                = new RESTore()
                .Given()
                .Headers(headers);

            Assert.That(context.HeaderAcceptCharset().Contains("utf-8"), Is.True);
        }

        [Test]
        [Category("Given Context : Methods")]
        public void CheckOtherHeaderMethodReturnsCorrectly()
        {
            GivenContext context
                = new RESTore()
                .Given()
                .Headers(headers);

            Assert.That(context.OtherHeaders().Count == 0, Is.True);
        }

        [Test]
        [Category("Given Context : Methods")]
        public void ValidateOtherHeaderLambdaForKey()
        {
            GivenContext context
                = new RESTore()
                .Given()
                .Headers(headers);

            Assert.That(context.RequestHeaders.ContainsKey("Accept"), Is.True);
        }

        [Test]
        [Category("Given Context : Methods")]
        public void ValidateOtherHeaderLambdaForValue()
        {
            GivenContext context
                = new RESTore()
                .Given()
                .Headers(headers);
            
            Assert.That(context.RequestHeaders.ContainsValue("application/json"), Is.True);
        }
    }
}
