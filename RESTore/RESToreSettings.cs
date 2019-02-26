using System;
using System.IO;

namespace Liberator.RESTore
{
    public static class RESToreSettings
    {
        public static TextWriter Log { get; set; } = Console.Out;
    }
}
