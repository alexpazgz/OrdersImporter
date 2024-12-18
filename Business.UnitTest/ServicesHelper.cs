using Businnes.Csv;
using Businnes.FireForgetService;
using Businnes.Implementations;
using Businnes.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Business.UnitTest
{
    public class ServicesHelper
    {
        private static IServiceProvider Provider()
        {
            var services = new ServiceCollection();

            services.AddScoped<ICsvService, CsvService>()
                    .AddScoped<ISummaryService, SummaryService>()
                    .AddScoped<IFireForgetService, FireForgetService>();

            return services.BuildServiceProvider();
        }

        public static T GetRequiredService<T>()
        {
            var provider = Provider();
            return provider.GetRequiredService<T>();
        }
    }
}
