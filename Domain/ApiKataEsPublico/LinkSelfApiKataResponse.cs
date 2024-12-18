using Newtonsoft.Json;

namespace Domain.ApiKataEsPublico
{
    public class LinkSelfApiKataResponse
    {
        [JsonProperty("self")]
        public string Self { get; set; }
    }
}
