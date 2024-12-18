using Newtonsoft.Json;

namespace Domain.ApiKataEsPublico
{
    public class LinkPageApiKataResponse : LinkSelfApiKataResponse
    {
        [JsonProperty("next")]
        public string Next { get; set; }

        [JsonProperty("prev")]
        public string Prev { get; set; }

    }
}
