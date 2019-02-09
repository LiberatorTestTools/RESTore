using System;

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
        public RESToreException(string message): base(message)
        {

        }
    }
}
