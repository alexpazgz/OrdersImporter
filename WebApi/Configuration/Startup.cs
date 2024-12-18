using Businnes.AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace WebApi.Configuration
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        internal static void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddCors();
            services.AddVersioning(configuration);
            services.AddCustomConfigurationOptions(configuration);
            services.AddDomainLayer(configuration);
            services.AddBusinnesLayer(configuration);
            services.AddSoapServiceReference(configuration);
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddInfrastructureServices(configuration);
     
        }
    }
}
