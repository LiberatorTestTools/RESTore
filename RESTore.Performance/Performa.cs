using Liberator.RESTore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Liberator.RESTore.Performance
{
    public class LoadTest
    {
        /// <summary>
        /// Allows a user to define the setup of a load profile for performance testing
        /// </summary>
        /// <returns>A new ProfileContext representing the new load profile to be made.</returns>
        public ProfileContext WithProfile()
        {
            string callingFunction = new StackTrace().GetFrame(1).GetMethod().Name;
            RESToreSettings.Log.WriteLine($"**BEGINNING OF TEST {callingFunction}**");
            return new ProfileContext();
        }
    }
}
