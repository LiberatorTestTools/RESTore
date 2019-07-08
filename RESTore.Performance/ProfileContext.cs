using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Liberator.RESTore.Performance
{
    public class ProfileContext : IProfileContext
    {
        public ProfileStep Step([Optional, DefaultParameterValue(0)]int stepNumber)
        {
            string callingFunction = new StackTrace().GetFrame(1).GetMethod().Name;
            RESToreSettings.Log.WriteLine($"**BEGINNING OF TEST {callingFunction}**");
            return new ProfileStep(this);
        }

        public PerformanceContext Perform()
        {
            return new PerformanceContext(this);
        }
    }

    public class ProfileStep
    {
        internal ProfileContext Context;
        internal int Users;
        internal double Requests;
        internal TimeSpan Duration;
        internal double Variance;
        internal double Timing;
        internal double Ramp;
        internal double BurstRate;
        internal TimeSpan BurstLength;

        public ProfileStep(ProfileContext context)
        {
            Context = context;
        }

        public ProfileStep UsersAtOnce(int usersAtOnce, [Optional, DefaultParameterValue(0)] double userVariance)
        {
            Users = usersAtOnce;
            return this;
        }

        public ProfileStep RequestRate(double requestRate, [Optional, DefaultParameterValue(0)]double rampingTarget,
            [Optional, DefaultParameterValue(0)]double requestVariance, [Optional, DefaultParameterValue(0)]double timingVariance)
        {
            Requests = requestRate;
            Ramp = rampingTarget;
            Timing = timingVariance;
            return this;
        }

        public ProfileStep Burst(double burstRate, TimeSpan burstLength)
        {
            BurstLength = burstLength;
            BurstLength = burstLength;
            return this;
        }

        public ProfileContext StepDuration(TimeSpan duration)
        {
            Duration = duration;
            return this.Context;
        }
    }
}
