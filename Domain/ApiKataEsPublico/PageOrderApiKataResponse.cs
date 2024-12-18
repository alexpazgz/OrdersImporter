using Newtonsoft.Json;

namespace Domain.ApiKataEsPublico
{
    public class PageOrderApiKataResponse
    {
        [JsonProperty("page")]
        public double Page { get; set; }

        [JsonProperty("content")]
        public List<OnlineOrderApiKataResponse> Content { get; set; }

        [JsonProperty("links")]
        public LinkPageApiKataResponse Links { get; set; }


        public bool CheckExistNextPage()
        {
            if (Links != null &&
                !string.IsNullOrEmpty(Links.Next))
            {
                return true;
            }

            return false;
        }

        public List<OnlineOrderApiKataResponse> GetOrders()
        {
            if (Content != null &&
                   Content.Any())
            {
                return Content;
            }

            return new List<OnlineOrderApiKataResponse>();
        }
    }
}
