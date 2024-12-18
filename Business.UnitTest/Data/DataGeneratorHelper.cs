using Domain.ApiKataEsPublico;
using Domain.Summary;
using Newtonsoft.Json;

namespace Business.UnitTest.Data
{
    public class DataGeneratorHelper
    {
        private const string URL_DATA = "Data";
        public PageOrderApiKataResponse GetPageOrderApiKataResponse()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, URL_DATA, "PageOrder.json");
            var json = File.ReadAllText(path);

            var pageOrderApiKataResponse = JsonConvert.DeserializeObject<PageOrderApiKataResponse>(json);

            if (pageOrderApiKataResponse == null)
                pageOrderApiKataResponse = new PageOrderApiKataResponse();

            return pageOrderApiKataResponse;
        }

        public List<OnlineOrderApiKataResponse> GetOnlineOrdersApiKataResponse()
        {
            var pageOrderApiKataResponse = GetPageOrderApiKataResponse();

            var onlineOrdersApiKataResponse = new List<OnlineOrderApiKataResponse>();

            if (pageOrderApiKataResponse == null)
                return onlineOrdersApiKataResponse;

            onlineOrdersApiKataResponse = pageOrderApiKataResponse.GetOrders();

            return onlineOrdersApiKataResponse;
        }

        public SummaryViewModel GetSummary()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, URL_DATA, "Summary.json");
            var json = File.ReadAllText(path);

            var summary = JsonConvert.DeserializeObject<SummaryViewModel>(json);

            if (summary == null)
                summary = new SummaryViewModel();

            return summary;
        }
    }
}
