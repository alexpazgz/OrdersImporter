using Newtonsoft.Json;
using System.Net;

namespace Domain.Response
{
    public class ApiResponseKo
    {
        [JsonProperty("error")]
        public string Error { get; private set; }

        [JsonProperty("trace")]
        public string Trace { get; private set; }

        [JsonProperty("type")]
        public string Type { get; private set; }

        [JsonProperty("status")]
        public HttpStatusCode Status { get; private set; }

        public ApiResponseKo(string error,
            string trace,
            string type,
            HttpStatusCode status)
        {
            Error = error;
            Trace = trace;
            Type = type;
            Status = status;
        }
    }
}
