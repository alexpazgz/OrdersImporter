using Newtonsoft.Json;

namespace Domain.Summary
{
    public class SummaryFieldTypeViewModel
    {
        [JsonProperty("region", NullValueHandling = NullValueHandling.Ignore)]
        public string? Region { get; set; }

        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string? Country { get; set; }

        [JsonProperty("itemType", NullValueHandling = NullValueHandling.Ignore)]
        public string? ItemType { get; set; }

        [JsonProperty("salesChannel", NullValueHandling = NullValueHandling.Ignore)]
        public string? SalesChannel { get; set; }

        [JsonProperty("orderPriority", NullValueHandling = NullValueHandling.Ignore)]
        public string? Priority { get; set; }

        [JsonProperty("cantidad")]
        public int Count { get; set; }
    }
}
