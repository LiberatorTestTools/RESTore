using NUnit.Framework;
using Liberator.RESTore.Enumerations;
using System.Collections.Generic;
using System.Linq;
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
                .Get()
                .Then()
                .AssertSuccessStatus()
                .ToConsole();
        }

        [Test]
        [Category("Then Context : Methods")]
        public void GetApiCall_StatusIsOK()
        {
            Assert.That(thenContext.StatusCode == HttpStatusCode.OK, Is.True);
        }

        [Test]
        [Category("Then Context : Methods")]
        public void GetApiCall_ContentLengthCorrect()
        {
            Assert.That(thenContext.Headers.ContainsKey("Content-Length"), Is.True);
            Assert.That(thenContext.Headers["Content-Length"].Contains("10883"), Is.True);
        }

        [Test]
        [Category("Then Context : Methods")]
        public void GetApiCall_ContentTypeCorrect()
        {
            Assert.That(thenContext.Headers.ContainsKey("Content-Type"), Is.True);
            Assert.That(thenContext.Headers["Content-Type"].Contains("text/html"), Is.True);
        }

        [Test]
        [Category("Then Context : Methods")]
        public void GetApiCall_LastModifiedReturned()
        {
            Assert.That(thenContext.Headers.ContainsKey("Last-Modified"), Is.True);
        }

        [Test]
        [Category("Then Context : Methods")]
        public void GetApiCall_IsSchemeValid()
        {
            Assert.That(thenContext.IsSchemaValid, Is.True);
        }
    }
}
