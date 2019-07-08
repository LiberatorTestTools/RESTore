using Liberator.RESTore.Enumerations;
using Liberator.RESTore.MIME;
using Liberator.RESTore.Performance;
using System;

namespace Liberator.RESTore.Tests
{
    public class ProfileContextTests
    {
        public void Test()
        {
            new LoadTest()
                .WithProfile()
                    .Step(0)
                        .UsersAtOnce(usersAtOnce: 10, userVariance: 0.10d)
                        .RequestRate(requestRate: 60d, rampingTarget: 90d, requestVariance: 0.150d, timingVariance: 0.100d)
                        .Burst(burstRate: 120, burstLength: new TimeSpan(0, 0, 10))
                        .StepDuration(duration: new TimeSpan(0, 1, 0))
                    .Step(1)
                        .UsersAtOnce(usersAtOnce: 15, userVariance: 0.05)
                        .RequestRate(requestRate: 90d, rampingTarget: 120d, requestVariance: 0.100d, timingVariance: 0.075d)
                        .Burst(burstRate: 180, burstLength: new TimeSpan(0, 0, 10))
                        .StepDuration(duration: new TimeSpan(0, 9, 0))
                .Perform()
                    .Given()
                        .Header(HeaderType.Accept, Application.Json)
                        .Host("http://www.totallyratted.com")
                    .When()
                        .Get();
        }
    }
}
