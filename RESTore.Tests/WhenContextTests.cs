using NUnit.Framework;
using Liberator.RESTore;
using System.Collections.Generic;

namespace Liberator.RESTore.Tests
{
    [TestFixture]
    public class WhenContextTests
    {
        [Test]
        [Category("When Context : Properties")]
        public void WhenStatement()
        {
            WhenContext context = new RESTore()
                .Given()
                .Name("Test suite name")
                .When();

            Assert.That(context.GivenContext.SuiteName.Equals("Test suite name"));
        }

        [Test]
        [Category("When Context : Properties")]
        public void LoadTestDefaultThreads()
        {
            WhenContext context = new RESTore()
                .Given()
                .Name("Test suite name")
                .When()
                .LoadTest();

            Assert.That(context.Threads == 1, Is.True);
        }

        [Test]
        [Category("When Context : Properties")]
        public void LoadTestDefaultSeconds()
        {
            WhenContext context = new RESTore()
                .Given()
                .Name("Test suite name")
                .When()
                .LoadTest();

            Assert.That(context.Seconds == 60, Is.True);
        }

        [Test]
        [Category("When Context : Executions")]
        public void GetRequestContextType()
        {
            ExecutionContext context = new RESTore()
                .Given()
                .Name("Test suite name")
                .Host("http://www.totallyratted.com")
                .When()
                .Get();

            Assert.That(context.GetType() == typeof(ExecutionContext), Is.True);
        }

        [Test]
        [Category("When Context : Executions")]
        public void GetHostException()
        {
            Assert.That(() =>
           {
               new RESTore()
               .Given()
               .Name("Test suite name")
               .When()
               .Get();
           }, Throws.ArgumentException);
        }

        [Test]
        [Category("When Context : Executions")]
        public void AddPathParams()
        {
            WhenContext when = new RESTore()
                .Given()
                    .Host("http://www.totallyratted.com")
                .When()
                    .PathParameter("id", "1")
                    .PathParameter("cow", "moo")
                    .PathParameters(new Dictionary<string, string>
                    {
                        {"mood", "happy"},
                        {"another", "one"},
                        {"extra", "param"}
                    });

            ExecutionContext context = when.Get("/{id}/url/{cow}/{mood}/something/{another}/end");

            Assert.That("http://www.totallyratted.com/1/url/moo/happy/something/one/end".Equals(when.TargetUrl), Is.True);
        }
    }
}
