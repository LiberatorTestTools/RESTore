using System;
using System.IO;

namespace Liberator.RESTore
{
    public static class RESToreSettings
    {
        /// <summary>
        /// The logger to be used for tests.
        /// </summary>
        public static TextWriter Log { get; set; } = Console.Out;
    }
}
