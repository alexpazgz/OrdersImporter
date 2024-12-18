using Businnes.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace OrdersImporterJob.Jobs
{
    public class JobGetOrders : IJobGetOrders
    {
        private readonly ILogger<JobGetOrders> _logger;
        private readonly IOrderImporterService _orderImporterService;

        public JobGetOrders(ILogger<JobGetOrders> logger,
            IOrderImporterService orderImporterService)
        {
            _logger = logger;
            _orderImporterService = orderImporterService;
        }
        public async Task Execute(string[] args)
        {
            _logger.LogInformation("Start process JobGetOrders");
            try
            {
                var resp = await _orderImporterService.GetOrdersWithoutThreadsAsync();

                var message = $"Resultado: {JsonConvert.SerializeObject(resp)}";
                _logger.LogInformation(message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            _logger.LogInformation("End process JobGetOrders");
        }
    }
}
