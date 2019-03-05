using NUnit.Framework;
using System;
using System.Diagnostics;

namespace Liberator.RESTore
{
    /// <summary>
    /// 
    /// </summary>
    public class RESToreException : Exception
    {
        /// <summary>
        /// An exception thrown by the library
        /// </summary>
        /// <param name="message">The message to send to the exception</param>
        public RESToreException(string message, Exception e) : base(message, e)
        {
            string callingFunction = new StackTrace().GetFrame(1).GetMethod().Name;
            RESToreSettings.Log.WriteLine($"The function {callingFunction} threw an exception: {message}");
            Assert.Fail();
        }
    }
}
