using Liberator.RESTore.Enumerations;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;

namespace Liberator.RESTore.Tests
{
    [TestFixture]
    public class ThenContextTests
    {
        private ThenContext thenContext;

        public ThenContextTests()
        {
            Dictionary<string, string> header = new Dictionary<string, string>()
            {
                {HeaderType.Accept, "application/json" }
            };

            thenContext = new RESTore()
                .Given()
                    .Headers(header)
                    .Name("Test suite name")
                    .Host("http://www.totallyratted.com")
                .When()
                    .Get("/index.html")
                .Then();
        }

        [Test]
        [Category("Then Context : Methods")]
        public void GetApiCall_StatusIsOK()
        {
            thenContext.AssertStatus(HttpStatusCode.OK);
        }

        [Test]
        [Category("Then Context : Methods")]
        public void GetApiCall_ContentLengthCorrect()
        {
            thenContext.AssertHeader(HeaderType.ContentLength, "10883");
        }

        [Test]
        [Category("Then Context : Methods")]
        public void GetApiCall_ContentTypeCorrect()
        {
            thenContext.AssertHeader(HeaderType.ContentType, "text/html");
        }

        [Test]
        [Category("Then Context : Methods")]
        public void GetApiCall_LastModifiedReturned()
        {
            Assert.That(thenContext.Headers.ContainsKey("Last-Modified"), Is.True);
        }

        [Test]
        [Category("Then Context : Methods")]
        public void GetApiCall_AssertSuccessStatus()
        {
            thenContext.AssertSuccessStatus();
        }

        [Test]
        [Category("Then Context : Methods")]
        public void GetApiCall_AssessBody()
        {
            Assert.That(() => thenContext.AssessBody<int>("test exception", number => number == 0), Throws.Exception.TypeOf<AssertionException>());
        }
    }
}
