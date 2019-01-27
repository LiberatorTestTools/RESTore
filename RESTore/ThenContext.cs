using System;
using System.Collections.Generic;
using System.Net;

namespace RESTore
{
    public class ThenContext
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Content { get; set; }
        public dynamic ParsedContent { get; set; }
        public Dictionary<string, IEnumerable<string>> Headers { get; set; }
        public TimeSpan ElapsedExecutionTime { get; set; }
        public Dictionary<string, double> LoadValues { get; set; }
        public Dictionary<string, bool> Assertions { get; set; }
        public List<LoadResponse> LoadResponses { get; set; }
        public bool IsSchemaValid { get; set; }
        public List<string> SchemaErrors { get; set; }

        public ThenContext()
        {
            Headers = new Dictionary<string, IEnumerable<string>>();
            LoadValues = new Dictionary<string, double>();
            Assertions = new Dictionary<string, bool>();
            IsSchemaValid = true;
            SchemaErrors = new List<string>();
        }

        public object Retrieve(Func<dynamic, object> func)
        {
            try
            {
                return func.Invoke(ParsedContent).Value;
            }
            catch { }

            try
            {
                return func.Invoke(ParsedContent);
            }
            catch { }

            return null;
        }
        
    }
}
