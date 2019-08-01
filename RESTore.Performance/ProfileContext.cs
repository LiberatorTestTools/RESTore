using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Liberator.RESTore.Performance
{
    /// <summary>
    /// The context used to hold the load profile being defined
    /// </summary>
    public class ProfileContext : IProfileContext
    {
        /// <summary>
        /// Allows a used to define a load profile step.
        /// </summary>
        /// <param name="stepNumber">The order in which to add the steps to the context.</param>
        /// <returns>The profile step context used to define a profile step.</returns>
        public ProfileStep Step([Optional, DefaultParameterValue(0)]int stepNumber)
        {
            string callingFunction = new StackTrace().GetFrame(1).GetMethod().Name;
            RESToreSettings.Log.WriteLine($"**BEGINNING OF TEST {callingFunction}**");
            return new ProfileStep(this);
        }

        /// <summary>
        /// Opens the performance context to allow the load test target to be defined
        /// </summary>
        /// <returns>The Performance Context representing the load test.</returns>
        public PerformanceContext Perform()
        {
            return new PerformanceContext(this);
        }
    }

    /// <summary>
    /// Used for the definition of individual profile steps
    /// </summary>
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

        /// <summary>
        /// Default contructor for the profile step context.
        /// </summary>
        /// <param name="context">The Profile context to use for the test.</param>
        public ProfileStep(ProfileContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Defines the number of virtual users to be used during the load test.
        /// </summary>
        /// <param name="usersAtOnce">The number of virtual users providing load.</param>
        /// <param name="userVariance">The variance percentage in the number of users.</param>
        /// <returns>The profile step context being defined.</returns>
        public ProfileStep UsersAtOnce(int usersAtOnce, [Optional, DefaultParameterValue(0)] double userVariance)
        {
            if (usersAtOnce > 0) { Users = usersAtOnce; } else { throw new RESToreException("RESTore performance testing requires at least one virtual user.", null); }
            if (Math.Abs(userVariance) < 0.5d) { Variance = Math.Abs(userVariance); } else { throw new RESToreException("User variance cannot exceed 50%", null); }
            return this;
        }

        /// <summary>
        /// Sets the request rate for the load test.
        /// </summary>
        /// <param name="requestRate">The initial request rate to be fired at the end point, stated in terms of requests per minute.</param>
        /// <param name="rampingTarget">The request rate to be reached at the end of the step, stated in terms of requests per minute.</param>
        /// <param name="requestVariance">The variance in the request rate, stated as a percentage of normal.</param>
        /// <param name="timingVariance">The variance in request timings, allowing a level of 'jitter' in the timing of requests</param>
        /// <returns>The profile step context being defined.</returns>
        public ProfileStep RequestRate(double requestRate, [Optional, DefaultParameterValue(0)]double rampingTarget,
            [Optional, DefaultParameterValue(0)]double requestVariance, [Optional, DefaultParameterValue(0)]double timingVariance)
        {
            if (requestRate > 0d) { Requests = requestRate; } else { throw new RESToreException("Cannot simulate a zero request load test.", null); }
            if (rampingTarget >= 0d) { Ramp = rampingTarget; } else { throw new RESToreException("Cannot reduce request rates to below zero.", null); }
            if (Math.Abs(requestVariance) < 0.5d) Timing = timingVariance;
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
