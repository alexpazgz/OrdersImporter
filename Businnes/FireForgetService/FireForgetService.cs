using Businnes.Csv;
using Businnes.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Businnes.FireForgetService
{
    public class FireForgetService : IFireForgetService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public FireForgetService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Execute(Func<IOnlineOrderService, Task> databaseWork)
        {
            // Fire off the task, but don't await the result
            Task.Run(async () =>
            {
                // Exceptions must be caught
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var service = scope.ServiceProvider.GetRequiredService<IOnlineOrderService>();

                    await databaseWork(service);
                }
                catch (Exception ex)
                {
                    //log
                }
            });
        }

        public void Execute(Func<ICsvService, Task> csvServiceWork)
        {
            // Fire off the task, but don't await the result
            Task.Run(async () =>
            {
                // Exceptions must be caught
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var service = scope.ServiceProvider.GetRequiredService<ICsvService>();

                    await csvServiceWork(service);
                }
                catch (Exception ex)
                {
                    //log
                }
            });
        }
    }
}
