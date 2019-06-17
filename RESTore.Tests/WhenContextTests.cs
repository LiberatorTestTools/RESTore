using NUnit.Framework;
using Liberator.RESTore;
using System.Collections.Generic;

namespace Liberator.RESTore.Tests
{
    [TestFixture]
    public class WhenContextTests
    {
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
    }
}
