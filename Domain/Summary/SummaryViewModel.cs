using Newtonsoft.Json;

namespace Domain.Summary
{
    public class SummaryViewModel
    {
        [JsonProperty("resumenPorRegion")]
        public List<SummaryFieldTypeViewModel> SummaryByRegion { get; set; }

        [JsonProperty("resumenPorCountry")]
        public List<SummaryFieldTypeViewModel> SummaryByCountry { get; set; }

        [JsonProperty("resumenPorItemType")]
        public List<SummaryFieldTypeViewModel> SummaryByItemType { get; set; }

        [JsonProperty("resumenPorSalesChannel")]
        public List<SummaryFieldTypeViewModel> SummaryBySalesChannel { get; set; }

        [JsonProperty("resumenPorOrderPriority")]
        public List<SummaryFieldTypeViewModel> SummaryByPriority { get; set; }

        public SummaryViewModel(List<SummaryFieldTypeViewModel> summaryByRegion,
            List<SummaryFieldTypeViewModel> summaryByCountry,
            List<SummaryFieldTypeViewModel> summaryByItemType,
            List<SummaryFieldTypeViewModel> summaryBySalesChannel,
            List<SummaryFieldTypeViewModel> summaryByPriority)
        {
            SummaryByRegion = summaryByRegion;
            SummaryByCountry = summaryByCountry;
            SummaryByItemType = summaryByItemType;
            SummaryBySalesChannel = summaryBySalesChannel;
            SummaryByPriority = summaryByPriority;
        }

        public SummaryViewModel()
        {
            SummaryByRegion = new List<SummaryFieldTypeViewModel>();
            SummaryByCountry = new List<SummaryFieldTypeViewModel>();
            SummaryByItemType = new List<SummaryFieldTypeViewModel>();
            SummaryBySalesChannel = new List<SummaryFieldTypeViewModel>();
            SummaryByPriority = new List<SummaryFieldTypeViewModel>();
        }
    }
}
