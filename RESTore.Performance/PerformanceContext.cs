using System;
using System.Collections.Generic;
using System.Text;

namespace Liberator.RESTore.Performance
{
    public class PerformanceContext:IPerformContext
    {
        internal ProfileContext ProfileContext { get; set; }

        public PerformanceContext(ProfileContext profileContext)
        {
            ProfileContext = profileContext;
        }

        public GivenContext Given()
        {
            return new GivenContext();
        }
    }
}
