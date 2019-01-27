using System;
using System.Collections;

namespace RESTore
{
    public static class Extensions
    {
        public static string FixProtocol(this string source, bool useHttps)
        {
            var defaultPortocol = useHttps ? "https" : "http";
            if (!source.StartsWith("http://") && !source.StartsWith("https://"))
                return $"{defaultPortocol}://" + source;

            return source;
        }

        public static string Quote(this string source)
        {
            return "\"" + source + "\"";
        }

        public static string SchemeAndHost(this Uri source)
        {
            return source.Scheme + "://" + source.Host;
        }

        public static bool IsPresentInDictionary(this IEnumerable dictionary, object value)
        {
            object objectFound = ((IDictionary)dictionary)[value];
            return objectFound != null ? true : false;
        }
    }
}
