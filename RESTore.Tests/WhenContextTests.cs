using NUnit.Framework;
using System;

namespace RESTore.Tests
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
        public void GetRequestConectType()
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
        public void GetRequestConectTypeException()
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
    }
}
