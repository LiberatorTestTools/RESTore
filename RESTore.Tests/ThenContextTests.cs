﻿using Liberator.RESTore.Enumerations;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Linq;

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

        [Test]
        [Category("Then Context : Methods")]
        public void GetApiCall_AssessHeader()
        {
            string test = "test1";
            thenContext.AssessHeader(test, HeaderType.ContentType, headerValues => headerValues.Contains("text/html"));
            thenContext.Assertions.TryGetValue(test, out bool value);
            Assert.That(value, Is.True);
        }

        [Test]
        [Category("Then Context : Methods")]
        public void GetApiCall_AssessHeaderWhereHeaderDoesntExist()
        {
            string test = "test2";
            string header = "moo";
            thenContext.AssessHeader(test, header, headerValues => headerValues.Contains("text/html"));
            thenContext.Assertions.TryGetValue(test, out bool value);
            Assert.That(value, Is.False);
        }

        [Test]
        [Category("Then Context : Methods")]
        public void GetApiCall_AssessHeaderException()
        {
            string test = "test3";
            Assert.That(() => thenContext.AssessHeader(test, HeaderType.ContentType, list => ((object)null).ToString().Equals("")),
                Throws.Exception.TypeOf<AssertionException>());
        }
    }
}
