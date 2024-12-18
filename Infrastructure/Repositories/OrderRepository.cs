using Businnes.Common.Interfaces;
using Businnes.Repositories.Interfaces;
using Domain.Configuration;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ILogger<OrderRepository> _logger;
        private readonly AppSettingsConfiguration _appSettingsConfiguration;

        private const int maxAttemps = 3;
        private TimeSpan initialDelay = TimeSpan.FromSeconds(1);

        public OrderRepository(IApplicationDbContext applicationDbContext,
            IOptions<AppSettingsConfiguration> appSettingsConfiguration,
            ILogger<OrderRepository> logger)
        {
            _applicationDbContext = applicationDbContext;
            _logger = logger;
            _appSettingsConfiguration = appSettingsConfiguration.Value;
        }

        public async Task InsertOrders(List<OnlineOrder> onlineOrders,
            CancellationToken cancellationToken = default)
        {
            int attemps;
            bool insertOrders;

            try
            {
                var totalOnlineOrders = onlineOrders.Count;
                var totalBulkEdit = _appSettingsConfiguration.BulkInsertRows;

                var timesToDo = Math.Ceiling((double)totalOnlineOrders / (double)totalBulkEdit);

                var message = $"Se van a realizar {timesToDo} BulkInsert en BBDD.";
                _logger.LogInformation(message);

                for (var i = 0; i < timesToDo; i++)
                {
                    insertOrders = false;
                    attemps = 0;

                    var currentOnlineOrders = onlineOrders.Skip(i * totalBulkEdit).Take(totalBulkEdit);

                    do
                    {
                        attemps++;
                        try
                        {
                            message = $"BulkInsert OK: {i}, itentos: {attemps}";
                            _logger.LogInformation(message);

                            await _applicationDbContext.OnlineOrder.BulkInsertAsync(currentOnlineOrders, options =>
                            {
                                options.IncludeGraph = true; // This ensures nested entities are included
                            },
                            cancellationToken);
                            insertOrders = true;
                        }
                        catch (Exception ex) when (attemps < maxAttemps)
                        {
                            message = $"Error BulkInsert: {i}, itentos: {attemps}, se reintenta.";
                            _logger.LogError(ex, message);
                            var delay = (int)Math.Pow(2, attemps) * initialDelay.TotalMilliseconds;
                            Thread.Sleep(TimeSpan.FromMilliseconds(delay));
                        }
                        catch (Exception ex) when (attemps == maxAttemps)
                        {
                            message = $"Error BulkInsert:{i} reintentos excedidos.";
                            _logger.LogError(ex, message);
                            insertOrders = false;
                        }
                    } while (!insertOrders && attemps < maxAttemps);

                    //await _applicationDbContext.OnlineOrder.BulkInsertAsync(currentOnlineOrders, options =>
                    //{
                    //    options.IncludeGraph = true; // This ensures nested entities are included
                    //},
                    //cancellationToken);
                }

                message = $"{timesToDo} BulkInsert realizados.";
                _logger.LogInformation(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en BulkIntent BBDD.");
            }
        }
    }
}
