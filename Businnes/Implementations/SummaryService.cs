using Businnes.Interfaces;
using Domain.Model;
using Domain.Summary;
using Microsoft.Extensions.Logging;

namespace Businnes.Implementations
{
    public class SummaryService : ISummaryService
    {
        private readonly ILogger<SummaryService> _logger;

        public SummaryService(ILogger<SummaryService> logger)
        {
            _logger = logger;
        }

        public SummaryViewModel Get(List<OnlineOrderModel> onlineOrdersModel)
        {
            SummaryViewModel summaryViewModel = new SummaryViewModel(); ;
            try
            {
                if (onlineOrdersModel != null && onlineOrdersModel.Any())
                {
                    var summaryByRegion = onlineOrdersModel.GroupBy(g => g.Region)
                        .Select(group => new SummaryFieldTypeViewModel
                        {
                            Region = group.Key,
                            Count = group.Count()
                        })
                        .OrderBy(o => o.Region)
                        .ToList();

                    var summaryByCountry = onlineOrdersModel.GroupBy(g => g.Country)
                        .Select(group => new SummaryFieldTypeViewModel
                        {
                            Country = group.Key,
                            Count = group.Count()
                        })
                        .OrderBy(o => o.Country)
                        .ToList();

                    var summaryByItemType = onlineOrdersModel.GroupBy(g => g.ItemType)
                        .Select(group => new SummaryFieldTypeViewModel
                        {
                            ItemType = group.Key,
                            Count = group.Count()
                        })
                        .OrderBy(o => o.ItemType)
                        .ToList();

                    var summaryBySalesChannel = onlineOrdersModel.GroupBy(g => g.SalesChannel)
                        .Select(group => new SummaryFieldTypeViewModel
                        {
                            SalesChannel = group.Key,
                            Count = group.Count()
                        })
                        .OrderBy(o => o.SalesChannel)
                        .ToList();

                    var summaryByOrderPriority = onlineOrdersModel.GroupBy(g => g.Priority)
                        .Select(group => new SummaryFieldTypeViewModel
                        {
                            Priority = group.Key,
                            Count = group.Count()
                        })
                        .OrderBy(o => o.Priority)
                        .ToList();

                    summaryViewModel = new SummaryViewModel(summaryByRegion,
                       summaryByCountry,
                       summaryByItemType,
                       summaryBySalesChannel,
                       summaryByOrderPriority);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar el resumen.");
            }
            return summaryViewModel;
        }
    }
}
